# Migration

Migration library for Unity that uses Odin serializer.
The library contains universal manager that migrates existing classes to their latest versions and serializer based on serialized versioned types.
The library uses reflection for type scanning and calling migration functions.
The migration manager also offers SHA-1 hash calculated from types to avoid unnecessary migration calls if no type was changed.

## Code

### Simple migration
```c#
// create migration manager with specific assembly your types are in
MigrationManager migration_manager = new MigrationManager(my_assembly);

// and recursively migrate all the types to their latest versions
migration_manager.Migrate(root_object);
```

### Change dependent migration
```c#
// read types hash together with data
byte[] types_hash = MyReadTypesHashFunction();

// migrate only if hash doesn't match
MigrationManager migration_manager = new MigrationManager(my_assembly);
if (force_migration || !migration_manager.TypesHash.SequenceEqual(types_hash))
{
    migration_manager.Migrate(root_object);
}

// later in serialization store migration_manager.TypesHash together with your data
```

## Data
The main advantage of the migration library is that serialized data structures used in the application remain untouched (if no custom identifiers are needed).

### Simple data class
Let's say we have simple class with one int variable.
We need first part of migration code for it:

```c#
    //---------------- Main code -----------------------
    [Serializable]
    public class SimpleClass
    {
        public int IntVariable;
    }
    
    //---------------- Migration code -----------------------
    // interface used for migration (all data classes that should be migrated needs this)
    [Preserve, MigratableInterface(typeof(SimpleClass))
    public interface ISimpleClassMigratable
    {
    }
    
    [Serializable, Preserve, LatestVersion(typeof(SimpleClass))]
    public class SimpleClass_000 : ISimpleClassMigratable // <-- note that _000 is important suffix to the class name, by that suffix migration manager knows the version of every class
    {
        public int IntVariable;
    }
```

Later, we need to rename ```IntVariable``` to ```StringVariable``` and change it's type to string.
The code will look like this:

```c#
    //---------------- Main code -----------------------
    [Serializable]
    public class SimpleClass
    {
        public string StringVariable;
    }
    
    //---------------- Migration code -----------------------
    // interface used for migration (all data classes that should be migrated needs this)
    [Preserve, MigratableInterface(typeof(SimpleClass))
    public interface ISimpleClassMigratable
    {
    }

    // class used for backward compatibility with older data
    [Serializable, Preserve, ObsoleteVersion(typeof(SimpleClass))]
    public class SimpleClass_000 : ISimpleClassMigratable
    {
        public int IntVariable;
    }

    // latest version of our data class with migration function that will be called when old version is deserialized
    [Serializable, Preserve, LatestVersion(typeof(SimpleClass))]
    public class SimpleClass_001 : ISimpleClassMigratable
    {
        public string StringVariable;
        
        public void Migrate(SimpleClass_000 prev)
        {
            StringVariable = prev.IntVariable.ToString();
        }
    }
```

At some point we want to rename ```StringVariable``` to just ```Variable```.

```c#
    //---------------- Main code -----------------------
    [Serializable]
    public class SimpleClass
    {
        public string Variable;
    }
    
    //---------------- Migration code -----------------------
    // interface used for migration (all data classes that should be migrated needs this)
    [Preserve, MigratableInterface(typeof(SimpleClass))
    public interface ISimpleClassMigratable
    {
    }

    // class used for backward compatibility with older data
    [Serializable, Preserve, ObsoleteVersion(typeof(SimpleClass))]
    public class SimpleClass_000 : ISimpleClassMigratable
    {
        public int IntVariable;
    }

    // class used for backward compatibility with older data
    [Serializable, Preserve, ObsoleteVersion(typeof(SimpleClass))]
    public class SimpleClass_001 : ISimpleClassMigratable
    {
        public string StringVariable;
        
        public void Migrate(SimpleClass_000 prev)
        {
            StringVariable = prev.IntVariable.ToString();
        }
    }

    // latest version of our data class with migration function that will be called when old version is deserialized
    [Serializable, Preserve, LatestVersion(typeof(SimpleClass))]
    public class SimpleClass_002 : ISimpleClassMigratable
    {
        public string Variable;
        
        public void Migrate(SimpleClass_001 prev)
        {
            Variable = prev.StringVariable;
        }
    }
```

In the last example, after ```SimpleClass_000``` is deserialized instance of ```SimpleClass_001``` is created and ```Migrate``` function is called on it.
After that new instance of class ```SimpleClass_002``` is created and it's ```Migrate``` function is called.
Then the latest version is serialized to memory and deserialized as main data class ```SimpleClass```.

The same principle can be used for structs in main data. They are mapped to migratable classes. 

### Complex data class

Let's assume we have following hierarchy:

```c#
    //---------------- Main code -----------------------
    [Serializable, SerializedClassId("custom-simple-class-id")] //<-- unique custom id that is used instead of class name in output, allows to rename classes freely
    public class SimpleClass
    {
        public int IntVariable;
    }
    
    [Serializable, SerializedClassId("custom-owner-class-id")] //<-- unique custom id that is used instead of class name in output, allows to rename classes freely
    public class OwnerClass
    {
        public SimpleClass       Instance;
        public List<SimpleClass> Instances;
    }
    
    //---------------- Migration code -----------------------
    // interface used for migration (all data classes that should be migrated needs this)
    [Preserve, MigratableInterface(typeof(SimpleClass))
    public interface ISimpleClassMigratable
    {
    }
    
    [Serializable, Preserve, LatestVersion(typeof(SimpleClass), 0)] //<-- in this case version is inside attribute, so the migratable class can be named independently
    public class SimpleClass_A : ISimpleClassMigratable
    {
        public int IntVariable;
    }

    // ... and for owner class
    [Preserve, MigratableInterface(typeof(OwnerClass))
    public interface IOwnerClassMigratable
    {
    }
    
    [Serializable, Preserve, LatestVersion(typeof(OwnerClass), 0)] //<-- in this case version is inside attribute, so the migratable class can be named independently
    public class OwnerClass_A : IOwnerClassMigratable
    {
        [SerializeReference] public ISimpleClassMigratable                     Instance;  //<-- note that SerializeReference is required!
        [SerializeReference, MigrateField] public List<ISimpleClassMigratable> Instances; //<-- force migration of this field, because List is not changing
    }
```
Now we want to rename ```IntVariable``` to just ```Variable```, ```Instance``` to ```SimpleInstance``` and ```Instances``` to ```SimpleInstances```.
```c#
    //---------------- Main code -----------------------
    [Serializable, SerializedClassId("custom-simple-class-id")]
    public class SimpleClass
    {
        public int IntVariable;
    }
    
    [Serializable, SerializedClassId("custom-owner-class-id")]
    public class OwnerClass
    {
        public SimpleClass       SimpleInstance;
        public List<SimpleClass> SimpleInstances;
    }
    
    //---------------- Migration code -----------------------
    [Preserve, MigratableInterface(typeof(SimpleClass))
    public interface ISimpleClassMigratable
    {
    }
    
    [Serializable, Preserve, ObsoleteVersion(typeof(SimpleClass), 0)]
    public class SimpleClass_A : ISimpleClassMigratable
    {
        public int IntVariable;
    }

    [Serializable, Preserve, LatestVersion(typeof(SimpleClass), 1)]
    public class SimpleClass_B : ISimpleClassMigratable
    {
        public int Variable;
        
        public void Migrate(SimpleClass_A prev)
        {
            Variable = prev.IntVariable;
        }
    }

    // ... and for owner class
    [Preserve, MigratableInterface(typeof(OwnerClass))
    public interface IOwnerClassMigratable
    {
    }
    
    [Serializable, Preserve, ObsoleteVersion(typeof(OwnerClass), 0)]
    public class OwnerClass_A : IOwnerClassMigratable
    {
        [SerializeReference] public ISimpleClassMigratable                     Instance;
        [SerializeReference, MigrateField] public List<ISimpleClassMigratable> Instances;
    }

    [Serializable, Preserve, LatestVersion(typeof(OwnerClass), 1)]
    public class OwnerClass_B : IOwnerClassMigratable
    {
        [SerializeReference] public ISimpleClassMigratable                     SimpleInstance;
        [SerializeReference, MigrateField] public List<ISimpleClassMigratable> SimpleInstances;
        
        public void Migrate(OwnerClass_A prev)
        {
            SimpleInstance = prev.Instance;
            SimpleInstances = prev.Instances; 
        }
    }
```

In this example migration of simple class is called first and owner class then.

**Note: every variable that represents a class in the main data must be marked as ```[SerializeReference]``` in migratable class and the original class type must be replaced by migratable empty interface.**

### Merge multiple classes into one

We have following data:

```c#
    //---------------------------------------------------------------
    // data
    //---------------------------------------------------------------
    [Serializable]
    public class Merge1Test
    {
        public int VariableA;
        
        public Merge1Test Init()
        {
            VariableA = 123;
            return this;
        }
    }
    
    //----
    [Serializable]
    public class Merge2Test
    {
        public string VariableA;

        public Merge2Test Init()
        {
            VariableA = "abc";
            return this;
        }
    }

    //---------------------------------------------------------------
    // migration
    //---------------------------------------------------------------
    [Preserve, MigratableInterface(typeof(Merge1Test))]
    public interface IMerge1TestMigratable
    {
        
    }
    
    [Serializable, Preserve, LatestVersion(typeof(Merge1Test), 0)]
    public class Merge1Test_A : IMerge1TestMigratable
    {
        public int VariableA;
    }

    //----
    [Preserve, MigratableInterface(typeof(Merge2Test))]
    public interface IMerge2TestMigratable
    {
        
    }

    [Serializable, Preserve, LatestVersion(typeof(Merge2Test), 0)]
    public class Merge2Test_A : IMerge2TestMigratable
    {
        public string VariableA;
    }
```

In new version of class ```Merge1Test``` we introduce a new string variable.
The class ```Merge2Test``` cease to exist after that change and we want it to be merged into new version of ```Merge1Test```.


```c#
    //---------------------------------------------------------------
    // data
    //---------------------------------------------------------------
    [Serializable]
    public class Merge1Test
    {
        public int    VariableA;
        public string VariableB;
        
        public Merge1Test Init()
        {
            VariableA = 123;
            VariableB = "abc";
            return this;
        }
    }
    
    // this class is merged into Merge1Test in new version and is no longer used
    // we need to keep it however to keep track of it's type
    [Serializable]
    public class Merge2Test
    {
    }
    
    //---------------------------------------------------------------
    // migration
    //---------------------------------------------------------------
    [Preserve, MigratableInterface(typeof(Merge1Test))]
    public interface IMerge1TestMigratable
    {
        
    }
    
    [Serializable, Preserve, ObsoleteVersion(typeof(Merge1Test), 0)]
    public class Merge1Test_A : IMerge1TestMigratable
    {
        public int VariableA;
    }

    [Serializable, Preserve, LatestVersion(typeof(Merge1Test), 1)]
    public class Merge1Test_B : IMerge1TestMigratable
    {
        public int    VariableA;
        public string VariableB;

        public void Migrate(Merge1Test_A prev)
        {
            VariableA = prev.VariableA;
            VariableB = "abc";
        }
    }
    
    [Preserve, MigratableInterface(typeof(Merge2Test))]
    public interface IMerge2TestMigratable
    {
        
    }

    [Serializable, Preserve, ObsoleteVersion(typeof(Merge2Test), 0)]
    public class Merge2Test_A : IMerge2TestMigratable, IMerge1TestMigratable // this class is merged into Merge1Test -> needs also that interface
    {
        public string VariableA;

        public object AllocateNext()
        {
            return
                new Merge1Test_B()
                {
                    VariableA = 123,
                    VariableB = this.VariableA
                };
        }
    }
```

### Split single class into multiple classes

Let's say we have following structure:

```c#
    //---------------------------------------------------------------
    // data
    //---------------------------------------------------------------
    
    [Serializable]
    public class Split1Test
    {
        public int VariableA;
        
        public Split1Test Init(int value)
        {
            VariableA = value;
            return this;
        }
    }
    
    //---------------------------------------------------------------
    // migration
    //---------------------------------------------------------------
    [Preserve, MigratableInterface(typeof(Split1Test))]
    public interface ISplit1TestMigratable
    {
        
    }
    
    [Serializable, Preserve, LatestVersion(typeof(Split1Test), 0)]
    public class Split1Test_A : ISplit1TestMigratable
    {
        public int VariableA;
    }
```

Now we want to split class ```Split1Test``` into two classes based on it's internal state.
In this case if value of ```Split1Test.VariableA``` is 1 it will remain as ```Split1Test```.
In other case it will became a ```Split2Test``` class.


```c#
    //---------------------------------------------------------------
    // data
    //---------------------------------------------------------------
    
    // this class is split into Split1Test or Split2Test by value of VariableA (1 or 2)
    [Serializable]
    public class Split1Test
    {
        public int VariableA;
        
        public Split1Test Init(int value)
        {
            VariableA = value;
            return this;
        }
    }
    
    //---- 
    [Serializable]
    public class Split2Test
    {
        public string VariableA;

        public Split2Test Init()
        {
            VariableA = "abc";
            return this;
        }
    }
    
    //---------------------------------------------------------------
    // migration
    //---------------------------------------------------------------
    [Preserve, MigratableInterface(typeof(Split1Test))]
    public interface ISplit1TestMigratable
    {
        
    }
    
    [Serializable, Preserve, ObsoleteVersion(typeof(Split1Test), 0)]
    public class Split1Test_A : ISplit1TestMigratable, ISplit2TestMigratable // ISplit2TestMigratable can be deserialized from this class too
    {
        public int VariableA;

        public object AllocateNext()
        {
            if (VariableA == 1)
            {
                return new Split1Test_B
                       {
                           VariableA = this.VariableA,
                           VariableB = "abc"
                       };
            }

            return new Split2Test_A
                   {
                       VariableA = "abc"
                   };
        }
    }

    [Serializable, Preserve, LatestVersion(typeof(Split1Test), 1)]
    public class Split1Test_B : ISplit1TestMigratable
    {
        public int    VariableA;
        public string VariableB;
    }

    //---- 
    [Preserve, MigratableInterface(typeof(Split2Test))]
    public interface ISplit2TestMigratable
    {
        
    }

    [Serializable, Preserve, LatestVersion(typeof(Split2Test), 0)]
    public class Split2Test_A : ISplit2TestMigratable
    {
        public string VariableA;
    }
```
