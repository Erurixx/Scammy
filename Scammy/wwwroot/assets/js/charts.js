$(function () {
    'use strict';

    // Generate longer random sample data that fills the box
    var smallChartData = [];
    var points = 30; // number of points
    for (var i = 0; i <= points; i++) {
        var x = (i / points) * 100; // scale x from 0 to 100
        var y = Math.floor(Math.random() * 30) + 5; // random y values
        smallChartData.push([x, y]);
    }

    // Small Flot Chart 1 (Line Chart)
    $.plot('#flotChart1', [{
        data: smallChartData,
        color: '#00cccc'
    }], {
        series: {
            shadowSize: 0,
            lines: {
                show: true,
                lineWidth: 2,
                fill: true,
                fillColor: { colors: [{ opacity: 0.2 }, { opacity: 0.2 }] }
            }
        },
        grid: {
            borderWidth: 0,
            labelMargin: 0
        },
        yaxis: {
            show: false,
            min: 0,
            max: Math.max(...smallChartData.map(d => d[1])) + 5
        },
        xaxis: {
            show: false,
            min: 0,
            max: 100 // scale to fill the width
        }
    });



    // Generate longer random sample data for "Articles Created"
    var articlesChartData = [];
    var points = 50; // number of points
    for (var i = 0; i <= points; i++) {
        var x = (i / points) * 100; // scale x from 0 to 100
        var y = Math.floor(Math.random() * 20) + 5; // random y values
        articlesChartData.push([x, y]);
    }

    // Small Flot Chart 2 (Line Chart)
    $.plot('#flotChart2', [{
        data: articlesChartData,
        color: '#007bff'
    }], {
        series: {
            shadowSize: 0,
            lines: {
                show: true,
                lineWidth: 2,
                fill: true,
                fillColor: { colors: [{ opacity: 0.2 }, { opacity: 0.2 }] }
            }
        },
        grid: {
            borderWidth: 0,
            labelMargin: 0
        },
        yaxis: {
            show: false,
            min: 0,
            max: Math.max(...articlesChartData.map(d => d[1])) + 5
        },
        xaxis: {
            show: false,
            min: 0,
            max: 100 // scale to fill the width
        }
    });

    // Generate random sample data for Total Articles Published
    var barData1 = [];
    var barData2 = [];
    var bars = 8; // number of bars
    for (var i = 0; i < bars; i++) {
        barData1.push(Math.floor(Math.random() * 50) + 10); // main bars
        barData2.push(Math.floor(Math.random() * 40) + 5);  // secondary bars
    }

    var ctx5 = document.getElementById('chartBar5');
    if (ctx5) {
        ctx5 = ctx5.getContext('2d');
        new Chart(ctx5, {
            type: 'bar',
            data: {
                labels: Array.from({ length: bars }, (_, i) => i + 1), // simple 1..8 labels
                datasets: [{
                    data: barData1,
                    backgroundColor: '#560bd0'
                }, {
                    data: barData2,
                    backgroundColor: '#cad0e8'
                }]
            },
            options: {
                maintainAspectRatio: false,
                plugins: {
                    tooltip: { enabled: false },
                    legend: { display: false }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        max: Math.max(...barData1, ...barData2) + 10,
                        display: false
                    },
                    x: {
                        barPercentage: 0.6,
                        grid: { color: 'rgba(0,0,0,0.08)' },
                        ticks: { display: false }
                    }
                }
            }
        });
    }

    // Generate random sample data for Total Reported Scams
    var points = 20; // number of data points
    var flotData1 = [];
    var flotData2 = [];
    for (var i = 0; i < points; i++) {
        flotData1.push([i, Math.floor(Math.random() * 60) + 20]); // main line
        flotData2.push([i, Math.floor(Math.random() * 50) + 15]); // secondary line
    }

    // Plot the chart
    $.plot('#flotChart', [
        {
            data: flotData1,
            color: '#007bff',
            lines: { show: true, fill: true, lineWidth: 2, fillColor: { colors: [{ opacity: 0.2 }, { opacity: 0.2 }] } }
        },
        {
            data: flotData2,
            color: '#560bd0',
            lines: { show: true, fill: true, lineWidth: 2, fillColor: { colors: [{ opacity: 0.1 }, { opacity: 0.1 }] } }
        }
    ], {
        series: { shadowSize: 0 },
        grid: { borderWidth: 0, labelMargin: 8 },
        yaxis: { show: true, min: 0, max: Math.max(...flotData1.map(d => d[1]), ...flotData2.map(d => d[1])) + 10 },
        xaxis: { show: true, min: 0, max: points - 1 }
    });
});
