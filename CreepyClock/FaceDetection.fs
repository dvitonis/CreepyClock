module FaceDetection

open Accord.Vision.Detection
open System.Drawing
open AForge.Imaging

    let findFaces (detector : HaarObjectDetector) (image : Bitmap) =
        let im = UnmanagedImage.FromManagedImage(image)
        let faces = detector.ProcessFrame im
        im.ToManagedImage() |> ignore
        faces