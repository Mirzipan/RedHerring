using RedHerring.Alexandria.Identifiers;

namespace RedHerring.Clues;

public interface DefinitionsQuery
{
    /// <summary>
    /// Returns all definitions of a given type.
    /// </summary>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type, if found, null otherwise</returns>
    IEnumerable<T> ByType<T>() where T : Definition;

    /// <summary>
    /// Returns a definition with the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type and id, if found, null otherwise</returns>
    T? ById<T>(CompositeId id) where T : Definition;

    /// <summary>
    /// Returns true if definition exists and assigns it to `definition`.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="definition"></param>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>True if definition exists</returns>
    bool TryById<T>(CompositeId id, out T? definition) where T : Definition;

    /// <summary>
    /// Returns true if definition exists.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>True if definition exists</returns>
    bool Contains<T>(CompositeId id) where T : Definition;

    /// <summary>
    /// Returns the default definition for the type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? Default<T>() where T : Definition;
}