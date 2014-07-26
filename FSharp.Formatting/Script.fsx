#I @"G:\GitHub\FSharp.Formatting\bin"
#r "FSharp.Literate.dll"
#r "FSharp.CodeFormat.dll"
#r "FSharp.Markdown.dll"
open FSharp.Literate
open FSharp.CodeFormat
open FSharp.Markdown

let md = """
## JavaScript

    [lang=js]
    var undefined;

    /** Used as the semantic version number */
    var VERSION = "3.0.0-pre";
    
    /** Used to compose bitmasks for wrapper metadata */
    var BIND_FLAG = 1,
        BIND_KEY_FLAG = 2,
        CURRY_FLAG = 4,
        CURRY_RIGHT_FLAG = 8,
        CURRY_BOUND_FLAG = 16,
        PARTIAL_FLAG = 32,
        PARTIAL_RIGHT_FLAG = 64;

    function copyWithEvaluation(iElem, elem) {
      return function (obj) {
          var newObj = {};
          for (var p in obj) {
              var v = obj[p];
              if (typeof v === "function") {
                  v = v(iElem, elem);
              }
              newObj[p] = v;
          }
          if (!newObj.exactTiming) {
              newObj.delay += exports._libraryDelay;
          }
          return newObj;
      };
    }

## C#

    [lang=cs]
    public MainViewModel(INavigationService navigationService, IEventAggregator eventAggregator
            , BingoCardViewModel bingoCardViewModel, BallCallsViewModel ballCallViewModel, PlayerInfoViewModel playerInfoViewModel, SoundViewModel soundViewModel)
            : base(navigationService)
    {
      _eventAggregator = eventAggregator;
      SoundPlayer = soundViewModel; 
      BingoCard = bingoCardViewModel;
      BallCalls = ballCallViewModel;
      PlayerInfo = playerInfoViewModel;
      
      // listen to Credits change in PlayerInfo
      PlayerInfo.PropertyChanged += (sender, args) =>
      {
          if (args.PropertyName == "Credits")
          {
              NotifyOfPropertyChange(() => CanPlay);
          }
      };

      x && y;
      x & y;
      x & y ? 1 : 0;
      x || y;
    }

"""

md
|> Literate.ParseMarkdownString 
|> Literate.WriteHtml

