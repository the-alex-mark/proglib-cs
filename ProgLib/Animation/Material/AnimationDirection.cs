namespace ProgLib.Animation.Material
{
    enum AnimationDirection
    {
        In, //In. Останавливается, если закончено.
        Out, //Out. Останавливается, если закончено.
        InOutIn, // То же, что и In, но изменяется на InOutOut, если закончено.
        InOutOut, // То же, что и Out.
        InOutRepeatingIn, // То же, что и In, но изменяется на InOutRepeatingOut, если закончено.
        InOutRepeatingOut // То же самое, что и Out, но изменяется на InOutRepeatingIn, если закончено.
    }
}
