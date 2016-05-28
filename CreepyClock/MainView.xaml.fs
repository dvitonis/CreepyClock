namespace Views

open FsXaml

type MainViewBase = XAML<"MainView.xaml">

type MainView() =
    inherit MainViewBase()

    override this.OnInitialize() =
        ignore ()
