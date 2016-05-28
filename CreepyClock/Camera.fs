module CreepyClock.Camera

open AForge.Video.DirectShow

let getDefaultCamera () =
    let videoDevices = new FilterInfoCollection( FilterCategory.VideoInputDevice )
    let cameraInfo = videoDevices.[0]
    let cameraSource = new VideoCaptureDevice(cameraInfo.MonikerString)
    cameraSource.VideoResolution <- cameraSource.VideoCapabilities.[0]
    cameraSource
