#ifndef WALL_DITHER_INCLUDED
#define WALL_DITHER_INCLUDED

void Dither4x4_float(
    float2 ScreenUV,
    float ClipValue,
    float Strength,
    out float Alpha)
{
    // Convert normalized screen coordinates into pixel coordinates.
    float2 pixelPos = ScreenUV * _ScreenParams.xy;

    // 4x4 Bayer index.
    uint x = (uint) pixelPos.x & 3;
    uint y = (uint) pixelPos.y & 3;

    const float Bayer[16] =
    {
        0.0, 8.0, 2.0, 10.0,
       12.0, 4.0, 14.0, 6.0,
        3.0, 11.0, 1.0, 9.0,
       15.0, 7.0, 13.0, 5.0
    };

    float threshold = Bayer[y * 4 + x] / 16.0;

    // Shift the Bayer threshold by the clip value.
    Alpha = step(threshold * Strength, ClipValue);
}

#endif