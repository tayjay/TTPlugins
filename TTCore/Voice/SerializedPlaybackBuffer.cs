namespace TTCore.Voice;

public struct SerializedPlaybackBuffer
{
    private float[] Buffer { get; set; }
    private long Length { get; set; }
    long Index { get; set; }
}