using System.Collections.Generic;
using UnityEngine;

public class BeatRecorder : MonoBehaviour
{
    public Conductor conductor; // Assign this in Inspector
    public KeyCode punchKey = KeyCode.E; // Example key for punching
    public KeyCode dodgeKey = KeyCode.A; // Example key for dodging
    public KeyCode generateOutputKey = KeyCode.Return; // Key to generate the final list

    // Store all beats that should NOT be missed
    private HashSet<int> safeBeats = new HashSet<int>();
    private int maxBeatRecorded = 0; // To keep track of the song's duration

    void Update()
    {
        // Get the current beat position
        int currentBeat = Mathf.RoundToInt(conductor.GetSongBeatPosition());

        // Update the maximum beat recorded
        if (currentBeat > maxBeatRecorded)
        {
            maxBeatRecorded = currentBeat;
        }

        // Record safe beats based on player actions
        if (Input.GetKeyDown(punchKey) || Input.GetKeyDown(dodgeKey))
        {
            // The startup beat (one beat before the hit)
            // This needs to be marked first to ensure the animation has time to play
            int startupBeat = currentBeat - 1;
            if (startupBeat >= 0 && !safeBeats.Contains(startupBeat)) // Ensure beat is not negative
            {
                safeBeats.Add(startupBeat);
                Debug.Log($"[Recorder] Marked Safe (Startup) Beat: {startupBeat}");
            }

            // The actual hit beat (where the key was pressed)
            if (!safeBeats.Contains(currentBeat))
            {
                safeBeats.Add(currentBeat);
                Debug.Log($"[Recorder] Marked Safe (Hit) Beat: {currentBeat}");
            }
        }

        // Generate the final beatsToMiss list when you press the output key
        if (Input.GetKeyDown(generateOutputKey))
        {
            GenerateMissedBeatsList();
        }
    }

    void GenerateMissedBeatsList()
    {
        Debug.Log("== Generated beatsToMiss List ==");
        // Iterate from beat 0 up to the maximum recorded beat
        // Adding a few extra beats at the end to ensure all potential misses are covered
        int startingAnalysisBeat = 0;

        for (int i = startingAnalysisBeat; i <= maxBeatRecorded + 2; i++)
        {
            if (!safeBeats.Contains(i))
            {
                Debug.Log($"AddBeatRangeToMiss({i}, {i});");
            }
        }
        Debug.Log("== End of beatsToMiss List ==");

        // Clear for next recording session if needed
        safeBeats.Clear();
        maxBeatRecorded = 0;
    }
}