namespace Syzygy.GameGeneration
{
    enum GenerationMessageType : byte
    {
        Unknown = 0,
        NewFixedBody,
        NewOrbitingBody,
        AssignPlayerToBody,
        FinishGenerating,
    }
}
