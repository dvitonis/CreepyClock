module CreepyClock.CompositionRoot

open Accord.Vision.Detection.Cascades
open Accord.Vision.Detection
open Camera
open FaceDetection
open Decision

let lookingDecider =
    let camera = getDefaultCamera ()
    let decisionFilter = LowPassDecisionFilter(windowSize = 6)
    let filterDecision = decisionFilter.Apply
    let faceDetector =
        let cascade = FaceHaarCascade();
        let detector =
            HaarObjectDetector(
                cascade = cascade,
                minSize = 25,
                searchMode = ObjectDetectorSearchMode.Average,
                scaleFactor = 1.2f,
                scalingMode = ObjectDetectorScalingMode.SmallerToGreater)
        detector.UseParallelProcessing <- true
        detector.Suppression <- 2;
        detector
    let findFaces' = findFaces faceDetector
    LookingDecider(camera, findFaces', filterDecision)