using System;
using VoiceChat.Networking;

namespace TTCore.Utilities;

public class AudioUtils
{
    public static VoiceMessage ModifyVoiceMessage(VoiceMessage message, float pitch = 1f, float volume = 1f)
    {
        if(pitch == 1f && volume == 1f)
            return message;
        
        
        // Create a new byte array of the same length to store the modified data
        byte[] modifiedData = new byte[message.DataLength]; 

        // Calculate a factor for optional volume adjustment
        float factor = volume;// * 1.0f / 24000.0f; // (Feel free to adjust the 24000.0f if needed)

        float sampleIndex = 0;
        for (int i = 0; i < message.DataLength; i++)
        {
            // Calculate the indices of the samples we'll interpolate between
            int sampleIndexFloor = (int)sampleIndex;
            int sampleIndexCeiling = sampleIndexFloor + 1;

            // Ensure we don't read outside the array bounds
            sampleIndexCeiling = Math.Min(sampleIndexCeiling, message.DataLength - 1); 

            // Calculate the interpolation factor (how much weight to give each sample)
            float interpolationFactor = sampleIndex - sampleIndexFloor;

            // Perform linear interpolation between the two samples
            byte interpolatedValue = (byte)(message.Data[sampleIndexFloor] * (1 - interpolationFactor) + 
                                            message.Data[sampleIndexCeiling] * interpolationFactor);

            // Optional: Apply volume adjustment
            interpolatedValue = (byte)(interpolatedValue * factor);

            // Store the modified sample in the output array
            modifiedData[i] = interpolatedValue; 
            sampleIndex += pitch;
        }
    
        // Create the new VoiceMessage with the modified data 
        return new VoiceMessage(message.Speaker, message.Channel, modifiedData, message.DataLength, 
            message.SpeakerNull);
    }

}