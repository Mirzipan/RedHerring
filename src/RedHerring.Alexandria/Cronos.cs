using System.Diagnostics;

namespace RedHerring.Alexandria;

public class Cronos
{
    private Stopwatch _stopwatch;

    private long _startTime;
    private long _elapsedTime;
    private long _totalTime;
    private long _lastTime;
    private bool _isPaused;

    public bool IsPaused => _isPaused;

    public long StartTime => _startTime;
    public long ElapsedTime => _elapsedTime;
    public long TotalTime => _totalTime;

    public Cronos() : this(0)
    {
    }

    public Cronos(long startTime)
    {
        _stopwatch = new Stopwatch();
        Reset();
        
        _startTime = startTime;
    }

    public void Tick()
    {
        if (_isPaused)
        {
            _elapsedTime = 0;
            return;
        }

        long time = _stopwatch.ElapsedMilliseconds;
        _totalTime = _startTime + time;
        _elapsedTime = time - _lastTime;

        _lastTime = time;
    }

    public void Reset()
    {
        _startTime = 0;
        _stopwatch.Reset();
    }

    public void Resume()
    {
        _isPaused = false;
    }

    public void Pause()
    {
        _isPaused = true;
    }
}