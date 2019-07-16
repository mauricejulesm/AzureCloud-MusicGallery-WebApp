using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using NAudio.Wave;

namespace MusicGallery_WebJob
{
    public class Functions
    {
        // This class contains the application-specific WebJob code consisting of event-driven
        // methods executed when messages appear in queues with any supporting code.

        // Trigger method  - run when new message detected in queue. "music-sample-maker" is name of queue.
        // ""music-gallery" is name of storage container; "musics" and "music-samples" are folder names. 
        // "{queueTrigger}" is an inbuilt variable taking on value of contents of message automatically;
        // the other variables are valued automatically.
        public static void GenerateMusicSample(
        [QueueTrigger("music-sample-maker")] String blobInfo,
        [Blob("music-gallery/musics/{queueTrigger}")] CloudBlockBlob inputBlob,
        [Blob("music-gallery/music-samples/{queueTrigger}")] CloudBlockBlob outputBlob, TextWriter logger)
        {
            //use log.WriteLine() rather than Console.WriteLine() for trace output
            logger.WriteLine("\n**** GenerateMusicSample() started...");
            logger.WriteLine("**** Input blob is: " + blobInfo);

            inputBlob.FetchAttributes();
            string metaBlobTitle = inputBlob.Metadata["Title"];


            // Open streams to blobs for reading and writing as appropriate.
            // Pass references to application specific methods
            using (Stream input = inputBlob.OpenRead())
            using (Stream output = outputBlob.OpenWrite())
            {
                MakeMusicSamples(input, output, 15);
                outputBlob.Properties.ContentType = "audio/mpeg3";
            }

            // setting the blobtitle
            outputBlob.Metadata["Title"] = metaBlobTitle;
            outputBlob.SetMetadata();
            logger.WriteLine("**** GenerateMusicSample() completed...");
        }



        // a method to change mp3 sample using the duration of 15 seconts. 

        private static void MakeMusicSamples(Stream input, Stream output, int duration)
        {
            using (var reader = new Mp3FileReader(input, wave => new NLayer.NAudioSupport.Mp3FrameDecompressor(wave)))
            {
                Mp3Frame frame;
                frame = reader.ReadNextFrame();
                int frameTimeLength = (int)(frame.SampleCount / (double)frame.SampleRate * 1000.0);
                int framesRequired = (int)(duration / (double)frameTimeLength * 1000.0);

                int frameNumber = 0;
                while ((frame = reader.ReadNextFrame()) != null)
                {
                    frameNumber++;

                    if (frameNumber <= framesRequired)
                    {
                        output.Write(frame.RawData, 0, frame.RawData.Length);
                    }
                    else break;
                }
            }
        }
    }
}