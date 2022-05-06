$(function () {

    var jsondatahourly = []
    var jsondatadaily = []
    var maxRxLvl = []
    var maxRxLvld = []
    var maxTxlvl = []
    var maxTxlvld = []
    var rsldeviation = []
    var rsldeviationd = []
    var timing = []
    var timingd = []
    var link = []
    var linkd = []
    var netype = []
    var netyped = []
  
 

    $.getJSON("https://localhost:44373/api/Fetch/Hourly-data", function (data) {
        for (var i in data) {
            jsondatahourly.push(data[i]);
            maxRxLvl.push(data[i].MAXRXLEVEL)
            maxTxlvl.push(data[i].MAXTXLEVEL)
            rsldeviation.push(data[i].RSL_DEVIATION_db)
            timing.push(data[i].TIME)
            link.push(data[i].LINK)
            netype.push(data[i].NETYPE)
        }
    })
    $.getJSON("https://localhost:44373/api/Fetch/Daily-data", function (data) {
        for (var i in data) {
            jsondatadaily.push(data[i]);
            maxRxLvld.push(data[i].MAXRXLEVEL)
            maxTxlvld.push(data[i].MAXTXLEVEL)
            rsldeviationd.push(data[i].RSL_DEVIATION_db)
            timingd.push(data[i].TIME)
            linkd.push(data[i].LINK)
            netyped.push(data[i].NETYPE)
        }
    });


    var data_Source = new kendo.data.DataSource({

        transport: {
            read: {
                url: "https://localhost:44373/api/Fetch/Hourly-data",
                dataType: "JSON"
            }
        },
        group: {
            field: "TIME"
        },
        schema: {
            model: {
                fields: {
                    Time: { type: "string" },
                    NE_TYPE: { type: "string" },
                    NE_ALIAS: { type: "string" },
                    LINK: { type: "string" },
                    MAXRXLEVEL: { type: "number" },
                    MAXTXLEVEL: { type: "number" },
                    RSL_DEVIATION_db: { type: "number" }
                }
            }
        },
        sort: {
            field: "Time",
            dir: "desc"
        },
    });
    data_Source.fetch(function () {

        /////////////////////////////////////////////////////////////CREATING THE CHART//////////////////////////////////////////////////////////////
        $("#chart").kendoChart({

            title: {

                text: "KPI-VARIATIONS"
            },

            legend: {
                position: "bottom"
            },

            seriesDefaults: {
                type: "line"
            },

            series: [{

                name: "MaxRxLevel",
                data: maxRxLvl
            },

            {
                name: "MaxTxLevel",
                data: maxTxlvl
            },

            {
                name: "RSL_DEVIATION",
                data: rsldeviation
            }

            ],

            valueAxis: {
                labels: {
                    format: "{0:hh:mm:ss}%"
                }

            },

            zoomable: true,

            categoryAxis: {

                categories: timing,
                type: "date",
                labels: {
                    dateFormats: {
                        days: "M-d"
                    }
                }
            },
        });


        /////////////////////////////////////////////////////////////CREATING THE CHART//////////////////////////////////////////////////////////////




        /////////////////////////////////////////////////////////////CREATING THE GRID//////////////////////////////////////////////////////////////
        // KPI values aggregated per datetime_key, NeType,NEAlias and LINK

        $("#grid").kendoGrid({
            columns: [{ title: "Time", field: "TIME" },
            // { title: "Network_SID", field: "NETWORK_SID" },
            { title: "NE_TYPE", field: "NETYPE" },
            { title: "NE_ALIAS", field: "NEALIAS" },
            { title: "LINK", field: "LINK" },
            { title: "MAXRXLEVEL", field: "MAXRXLEVEL" },
            { title: "MAXTXLEVEL", field: "MAXTXLEVEL" },
            { title: "RSL_DEVIATION_db", field: "RSL_DEVIATION_db" }],

            dataSource: {
                data: jsondatahourly,
            },
            height: 400,
            pageable: {
                pageSize: 25
            },
            sortable: true,
            filterable: true,
        });
        /////////////////////////////////////////////////////////////CREATING THE GRID//////////////////////////////////////////////////////////////
    });















});
/////////////////////////////////////////////////////////////CREATING THE RADIOBUTTON-FUNCTION//////////////////////////////////////////////////////////////
function getValue(radio) {
    var chart = $("#chart").data("kendoChart");
    chart.refresh();
    fetcheddata = []
    fetchedmaxrxlev = []
    fetchedmaxtxlev = []
    fetchedrsldev = []
    fetchedtime = []
    $.getJSON("https://localhost:44373/api/Fetch/" + radio.value, function (data) {
        for (var i in data) {
            fetcheddata.push(data[i]);
            fetchedmaxrxlev.push(data[i].MAXRXLEVEL)
            fetchedmaxtxlev.push(data[i].MAXTXLEVEL)
            fetchedrsldev.push(data[i].RSL_DEVIATION_db)
            fetchedtime.push(data[i].TIME)
        }
    });

    var data_Source = new kendo.data.DataSource({

        transport: {
            read: {
                url: "https://localhost:44373/api/Fetch/" + radio.value,
                dataType: "JSON"
            }
        },

        schema: {
            model: {
                fields: {
                    Time: { type: "string" },
                    NE_TYPE: { type: "string" },
                    NE_ALIAS: { type: "string" },
                    LINK: { type: "string" },
                    MAXRXLEVEL: { type: "number" },
                    MAXTXLEVEL: { type: "number" },
                    RSL_DEVIATION_db: { type: "number" }
                }
            }
        },

        sort: {
            field: "Time",
            dir: "desc"
        },
    });


    data_Source.fetch(function () {
        $("#chart").kendoChart({

            title: {
                text: "KPI-VARIATIONS"
            },

            legend: {
                position: "bottom"
            },

            seriesDefaults: {
                type: "line"
            },

            series: [{
                name: "MaxRxLevel",
                data: fetchedmaxrxlev
            },

            {
                name: "MaxTxLevel",
                data: fetchedmaxtxlev
            },

            {
                name: "RSL_DEVIATION",
                data: fetchedrsldev
            }],

            valueAxis: {
                labels: {
                    format: "{0:hh:mm:ss}%"
                }

            },
            zoomable: true,

            categoryAxis: {

                categories: fetchedtime,
                type: "date",
                labels: {
                    dateFormats: {
                        days: "M-d"
                    }
                }
            },


        });
        $("#grid").kendoGrid({
            dataSource: {
                data: fetcheddata,

            },
            height: 400,
            pageable: {
                pageSize: 25
            },
            sortable: true,
            filterable: true,
            columns: [{
                title: "Time",
                field: "TIME"
            }, {
                title: "NE_TYPE",
                field: "NETYPE"
            }, {
                title: "NE_ALIAS",
                field: "NEALIAS"
            }, {
                title: "LINK",
                field: "LINK"
            }, {
                title: "MAXRXLEVEL",
                field: "MAXRXLEVEL"
            }, {
                title: "MAXTXLEVEL",
                field: "MAXTXLEVEL"
            }, {
                title: "RSL_DEVIATION_db",
                field: "RSL_DEVIATION_db"
            }]




        });
    });
}
////////////////////////////////////////////////////////////////////
function getnealias(radio) {
    var chart = $("#chart").data("kendoChart");
    chart.refresh();
    var select = document.getElementById("link");
    fetcheddata = []
    fetchedmaxrxlev = []
    fetchedmaxtxlev = []
    fetchedrsldev = []
    fetchedtime = []
    links = []


    $.getJSON("https://localhost:44373/api/Fetch/Hourly-data/" + radio.value, function (data) {
        for (var i in data) {
            fetcheddata.push(data[i]);
            fetchedmaxrxlev.push(data[i].MAXRXLEVEL)
            fetchedmaxtxlev.push(data[i].MAXTXLEVEL)
            fetchedrsldev.push(data[i].RSL_DEVIATION_db)
            fetchedtime.push(data[i].TIME)
            links.push(data[i].LINK)
        }
    });
   
    

    var data_Source = new kendo.data.DataSource({

        transport: {
            read: {
                url: "https://localhost:44373/api/Fetch/Hourly-data/" + radio.value,
                dataType: "JSON"
            }
        },

        schema: {
            model: {
                fields: {
                    Time: { type: "string" },
                    NE_TYPE: { type: "string" },
                    NE_ALIAS: { type: "string" },
                    LINK: { type: "string" },
                    MAXRXLEVEL: { type: "number" },
                    MAXTXLEVEL: { type: "number" },
                    RSL_DEVIATION_db: { type: "number" }
                }
            }
        },

        sort: {
            field: "Time",
            dir: "desc"
        },
    });


    data_Source.fetch(function () {
        $("#chart").kendoChart({

            title: {
                text: "KPI-VARIATIONS"
            },

            legend: {
                position: "bottom"
            },

            seriesDefaults: {
                type: "line"
            },

            series: [{
                name: "MaxRxLevel",
                data: fetchedmaxrxlev
            },

            {
                name: "MaxTxLevel",
                data: fetchedmaxtxlev
            },

            {
                name: "RSL_DEVIATION",
                data: fetchedrsldev
            },
            {
                name: "FirstLINK",
                data: links[1]
            }

            ],

            valueAxis: {
                labels: {
                    format: "{0:hh:mm:ss}%"
                }

            },
            zoomable: true,

            categoryAxis: {

                categories: fetchedtime,
                type: "date",
                labels: {
                    dateFormats: {
                        days: "M-d"
                    }
                }
            },


        });
        $("#grid").kendoGrid({
            dataSource: {
                data: fetcheddata,

            },
            height: 400,
            pageable: {
                pageSize: 25
            },
            sortable: true,
            filterable: true,
            columns: [{
                title: "Time",
                field: "TIME"
            }, {
                title: "NE_TYPE",
                field: "NETYPE"
            }, {
                title: "NE_ALIAS",
                field: "NEALIAS"
            }, {
                title: "LINK",
                field: "LINK"
            }, {
                title: "MAXRXLEVEL",
                field: "MAXRXLEVEL"
            }, {
                title: "MAXTXLEVEL",
                field: "MAXTXLEVEL"
            }, {
                title: "RSL_DEVIATION_db",
                field: "RSL_DEVIATION_db"
            }]




        });
    });
    var distinctlinks=[]
    $.getJSON("https://localhost:44373/api/Fetch/GetLinks/" + radio.value, function (data) {
        for (var i in data) {
            distinctlinks.push(data[i].LINK)
        }
    });

    console.log(distinctlinks)

    var dropdown = document.getElementById("select");

    // Loop through the array
    for (var i = 0; i < distinctlinks.length; ++i) {
        // Append the element to the end of Array list
        dropdown[dropdown.length] = new Option(distinctlinks[i], distinctlinks[i]);
    }
    

    
}



/////////////////////////////////////////////////////////////CREATING THE RADIOBUTTON-FUNCTION//////////////////////////////////////////////////////////////