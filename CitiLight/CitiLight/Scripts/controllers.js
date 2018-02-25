'use strict';

angular.module('app.controllers', [])

    // Path: /
    .controller('HomeCtrl', ['$scope', '$location', '$window', '$http', '$interval', '$anchorScroll',
        function ($scope, $location, $window, $http, $interval, $anchorScroll) {
            $scope.$root.title = 'Citi Lightning';

            //// STRATEGY ////

            //Gets strategy queue table //
            //$http.get('http://query.yahooapis.com/v1/public/yql?q=select+*+from+csv+where+url%3d%27http%3a%2f%2fdownload.finance.yahoo.com%2fd%2fquotes.csv%3fs%3dYHOO%2cGOOG%2cAAPL%26f%3dsnxl1d1t1c1ohgpvers1k1bak2v1g4g1j3c8i5mwj5k4j6k5m3m7m8m4m5m6%26e%3d.csv%27and+columns%3d%27Symbol%2cName%2cStockExchange%2cPrice%2cDate%2cTime%2cChange%2cOpen%2cHigh%2cLow%2cClose%2cVolume%2cEarnings%2fShare%2cPeRatio%2cSharesOwned%2cLastTradeTime%2cBid%2cAsk%2cPctChange%2cHoldingsValue%2cHoldingsGain%2cHoldginsGainPct%2cMarketCap%2cAfterHoursChange%2cOrderBook%2cDayRange%2c52WeekRange%2cChange52WeekLow%2cChange52WeekHigh%2cPctChange52WkLow%2cPctChange52WkHigh%2c50DayMovingAvg%2cChange50DayMoving%2cPctChange50DayMoving%2c200DayMovingAvg%2cChange200DayMoving%2cPctChange200DayMoving%27&format=json&diagnostics=false').success(function (data) {
            //    $scope.CartData = data.query.results.row;
            //});

            $http.get('http://localhost:56728/api/Ticker/ABBV').success(function (data) {
                console.log(data[0].name);
                console.log("GET FROM API " + data);
            });


            GetStrategyQueueTable();

            function GetStrategyQueueTable() {
                $http.get('http://localhost:56728/api/Strategy').success(function (data) {
                    $scope.CartData = data;
                    UpdatePortfolio(data);
                });
            }
            function UpdatePortfolio(data) {
                var PieData = [
                    {
                     value: 34,
                     color: "#F7464A",
                     highlight: "#FF5A5E",
                     label: "$C"
                    },
                    {
                    value: 50,
                    color: "#46BFBD",
                     highlight: "#5AD3D1",
                    label: "$APPL"
                    },
                    {
                    value: 100,
                    color: "#FDB45C",
                    highlight: "#FFC870",
                    label: "$GOOG"
                }];

                var countries = document.getElementById("PieChartPortfolio").getContext("2d");
                new Chart(countries).Pie(PieData);
            }

            //Gets all tickers//
            //$scope.searchTicker = function () {
            //    $http.get('http://localhost:56728/api/Ticker/C').success(function (data) {
            //        $scope.tick = data;
            //        console.log(data);
            //    });
            //}

            $scope.Play = function Play(Id, ticker, shortPosition, longPosition, percentChange, numberOfShares) {


                $http.put('http://localhost:56728/api/Strategy/' + Id + '',
                    {
                        Id: Id,
                        Ticker: ticker,
                        LongPosition: longPosition,
                        ShortPosition: shortPosition,
                        NumberOfShares: numberOfShares,
                        PercentChange: percentChange,
                        StrategyActive: 1,
                        UserId: 1
                    }).
                then(function (response) {
                    GetStrategyQueueTable();
                }, function (response) {;
                    alert('Could not execute strategy');
                });

            }






            $scope.Stop = function Stop(Id, ticker, shortPosition, longPosition, percentChange, numberOfShares) {


                $http.put('http://localhost:56728/api/Strategy/' + Id + '',
                    {
                        Id: Id,
                        Ticker: ticker,
                        LongPosition: longPosition,
                        ShortPosition: shortPosition,
                        NumberOfShares: numberOfShares,
                        PercentChange: percentChange,
                        StrategyActive: 0,
                        UserId: 1
                    }).
                then(function (response) {
                    GetStrategyQueueTable();
                }, function (response) {;
                    alert('Could not execute strategy');
                });

            }

            $scope.AddStrategy = function (StrategyTicker) {

                $http.post('http://localhost:56728/api/Strategy',
                    {
                        Ticker: StrategyTicker,
                        StrategyActive: 0,
                        UserId: 1
                    }).
                    then(function (response) {
                        GetStrategyQueueTable();
                    }, function (response) {
                        alert('Could not add strategy to the queue');
                    });
            }

            $scope.DeleteStrategy = function (StrategyTicker) {

                $http.delete('http://localhost:56728/api/Strategy/'+StrategyTicker+'').
                    then(function (response) {
                    GetStrategyQueueTable();
                    }, function (response) {
                     alert('Could not delete strategy from the queue');
                     });
                    }
            


            //// END STRATEGY ////


            /// TRANSACTION TABLE ///

                $interval(function () {
                    $http.get('http://localhost:56728/api/Transaction').success(function (data) {
                        $scope.TransactionTableData = data;
                    });
                }, 5000);

            /// END TRANSACTION TABLE ///


            //// LIVE MARKET DATA ////


                var drawGraph;

            $scope.LoadGraph = function (ticker) {
                $interval.cancel(drawGraph);
                $http.get('http://query.yahooapis.com/v1/public/yql?q=select+*+from+csv+where+url%3d%27http%3a%2f%2fdownload.finance.yahoo.com%2fd%2fquotes.csv%3fs%3d'+ticker+'%26f%3dsnl1c1orj1%26e%3d.csv%27and+columns%3d%27Symbol%2cName%2cPrice%2cChange%2cOpen%2cPeRatio%2cMarketCap%27&format=json&diagnostics=false').success(function (data) {
                    $scope.TickerSpecific = data.query.results.row;
                    console.log(data);
                });
                var GraphData;
                var PriceHold;
                var TimeHold;
                $http.get('http://query.yahooapis.com/v1/public/yql?q=select+*+from+csv+where+url%3d%27http%3a%2f%2fdownload.finance.yahoo.com%2fd%2fquotes.csv%3fs%3d' + ticker + '%26f%3dsl1t1%26e%3d.csv%27and+columns%3d%27Symbol%2cPrice%2cTime%27&format=json&diagnostics=false').success(function (data) {
                    GraphData = data.query.results.row;
                    angular.forEach(GraphData, function (value, key) {
                        if (key == 'Price') {
                            PriceHold = value;
                        } else if (key == 'Time') {
                            TimeHold = value;
                        }
                        else;
                    });
                });



                var ChartData = {
                    labels: ["", "", "", "", "", "", "", "", "", ""],
                    datasets: [
                      {
                          fillColor: "#FFFF66",
                          strokeColor: "rgba(220,220,220,1)",
                          pointColor: "rgba(220,220,220,1)",
                          pointStrokeColor: "#fff",
                          data: [0, 0, 0, 0, 0, 0,0,0,0,0]
                      },
                    ]
                }

                var updateData = function (oldData) {
                    var labels = oldData["labels"];
                    var dataSetA = oldData["datasets"][0]["data"];

                    $http.get('http://query.yahooapis.com/v1/public/yql?q=select+*+from+csv+where+url%3d%27http%3a%2f%2fdownload.finance.yahoo.com%2fd%2fquotes.csv%3fs%3d' + ticker + '%26f%3dsl1t1%26e%3d.csv%27and+columns%3d%27Symbol%2cPrice%2cTime%27&format=json&diagnostics=false').success(function (data) {
                        GraphData = data.query.results.row;
                        angular.forEach(GraphData, function (value, key) {
                            if (key == 'Price') {
                                PriceHold = value;
                            } else if (key == 'Time') {
                                TimeHold = value;
                            }
                            else;
                        });
                    });


                    labels.shift();
                    labels.push(TimeHold.toString());
                    dataSetA.push(parseFloat(PriceHold));
                    dataSetA.shift();
                };

                var options = {
                    animation: false,         
                }


                var ctx = document.getElementById("MarketDataGraph").getContext("2d");
                var myNewChart = new Chart(ctx);
                myNewChart.Line(ChartData, options);


               drawGraph =  $interval(function() {
                    updateData(ChartData);
                    myNewChart.Line(ChartData, options);
                }, 5000);

            }

            //// END LIVE MARKET DATA ////



            //// PORTFOLIO ////


            $http.get('http://query.yahooapis.com/v1/public/yql?q=select+*+from+csv+where+url%3d%27http%3a%2f%2fdownload.finance.yahoo.com%2fd%2fquotes.csv%3fs%3dYHOO%2cGOOG%2cAAPL%26f%3dsnxl1d1t1c1ohgpvers1k1bak2v1g4g1j3c8i5mwj5k4j6k5m3m7m8m4m5m6%26e%3d.csv%27and+columns%3d%27Symbol%2cName%2cStockExchange%2cPrice%2cDate%2cTime%2cChange%2cOpen%2cHigh%2cLow%2cClose%2cVolume%2cEarnings%2fShare%2cPeRatio%2cSharesOwned%2cLastTradeTime%2cBid%2cAsk%2cPctChange%2cHoldingsValue%2cHoldingsGain%2cHoldginsGainPct%2cMarketCap%2cAfterHoursChange%2cOrderBook%2cDayRange%2c52WeekRange%2cChange52WeekLow%2cChange52WeekHigh%2cPctChange52WkLow%2cPctChange52WkHigh%2c50DayMovingAvg%2cChange50DayMoving%2cPctChange50DayMoving%2c200DayMovingAvg%2cChange200DayMoving%2cPctChange200DayMoving%27&format=json&diagnostics=false').success(function (data) {
                $scope.data2 = data.query.results.row;
                $interval(GetStockData, 2000);
            });

            function GetStockData() {
                $http.get('http://query.yahooapis.com/v1/public/yql?q=select+*+from+csv+where+url%3d%27http%3a%2f%2fdownload.finance.yahoo.com%2fd%2fquotes.csv%3fs%3dYHOO%2cGOOG%2cAAPL%26f%3dsnxl1d1t1c1ohgpvers1k1bak2v1g4g1j3c8i5mwj5k4j6k5m3m7m8m4m5m6%26e%3d.csv%27and+columns%3d%27Symbol%2cName%2cStockExchange%2cPrice%2cDate%2cTime%2cChange%2cOpen%2cHigh%2cLow%2cClose%2cVolume%2cEarnings%2fShare%2cPeRatio%2cSharesOwned%2cLastTradeTime%2cBid%2cAsk%2cPctChange%2cHoldingsValue%2cHoldingsGain%2cHoldginsGainPct%2cMarketCap%2cAfterHoursChange%2cOrderBook%2cDayRange%2c52WeekRange%2cChange52WeekLow%2cChange52WeekHigh%2cPctChange52WkLow%2cPctChange52WkHigh%2c50DayMovingAvg%2cChange50DayMoving%2cPctChange50DayMoving%2c200DayMovingAvg%2cChange200DayMoving%2cPctChange200DayMoving%27&format=json&diagnostics=false').success(function (data) {
                    $scope.data2 = data.query.results.row;
                });
            }

            $scope.setColorIndividualTicker = function (item) {
                if (item > 0) {
                    return { color: "green" }

                } else {
                    return { color: "red" }
                }
            }

            $scope.setColor = function (item) {
                if (item.Change > 0) {
                    return { color: "green" }

                } else {
                    return { color: "red" }
                }
            }

            //// END PORTFOLIO ////
        }])


