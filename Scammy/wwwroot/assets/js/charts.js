$(function () {
    'use strict'

    // Define sample data for Flot charts
    var flotSampleData3 = [
        [0, 20], [10, 25], [20, 35], [30, 30], [40, 45], [50, 40], [60, 50], [70, 45], [80, 55], [90, 50], [100, 60]
    ];

    var flotSampleData4 = [
        [0, 15], [10, 20], [20, 25], [30, 35], [40, 30], [50, 35], [60, 40], [70, 35], [80, 45], [90, 40], [100, 50]
    ];

    var dashData2 = [
        [0, 10], [5, 15], [10, 20], [15, 25], [20, 30], [25, 25], [30, 35], [35, 30], [40, 25], [45, 20], [50, 15]
    ];

    // Main Flot Chart
    var plot = $.plot('#flotChart', [{
        data: flotSampleData3,
        color: '#007bff',
        lines: {
            fillColor: { colors: [{ opacity: 0 }, { opacity: 0.2 }] }
        }
    }, {
        data: flotSampleData4,
        color: '#560bd0',
        lines: {
            fillColor: { colors: [{ opacity: 0 }, { opacity: 0.2 }] }
        }
    }], {
        series: {
            shadowSize: 0,
            lines: {
                show: true,
                lineWidth: 2,
                fill: true
            }
        },
        grid: {
            borderWidth: 0,
            labelMargin: 8
        },
        yaxis: {
            show: true,
            min: 0,
            max: 100,
            ticks: [[0, ''], [20, '20K'], [40, '40K'], [60, '60K'], [80, '80K']],
            tickColor: '#eee'
        },
        xaxis: {
            show: true,
            color: '#fff',
            ticks: [[25, 'OCT 21'], [75, 'OCT 22'], [100, 'OCT 23'], [125, 'OCT 24']],
        }
    });

    // Small Flot Charts
    $.plot('#flotChart1', [{
        data: dashData2,
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
            max: 35
        },
        xaxis: {
            show: false,
            max: 50
        }
    });

    $.plot('#flotChart2', [{
        data: dashData2,
        color: '#007bff'
    }], {
        series: {
            shadowSize: 0,
            bars: {
                show: true,
                lineWidth: 0,
                fill: 1,
                barWidth: .5
            }
        },
        grid: {
            borderWidth: 0,
            labelMargin: 0
        },
        yaxis: {
            show: false,
            min: 0,
            max: 35
        },
        xaxis: {
            show: false,
            max: 20
        }
    });

    // Initialize Peity charts
    $('.peity-line').peity('line');
    $('.peity-bar').peity('bar');
    $('.peity-donut').peity('donut');

    // Chart.js Bar Chart
    var ctx5 = document.getElementById('chartBar5').getContext('2d');
    new Chart(ctx5, {
        type: 'bar',
        data: {
            labels: [0, 1, 2, 3, 4, 5, 6, 7],
            datasets: [{
                data: [2, 4, 10, 20, 45, 40, 35, 18],
                backgroundColor: '#560bd0'
            }, {
                data: [3, 6, 15, 35, 50, 45, 35, 25],
                backgroundColor: '#cad0e8'
            }]
        },
        options: {
            maintainAspectRatio: false,
            plugins: {
                tooltip: {
                    enabled: false
                },
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    display: false,
                    beginAtZero: true,
                    max: 80
                },
                x: {
                    barPercentage: 0.6,
                    grid: {
                        color: 'rgba(0,0,0,0.08)'
                    },
                    ticks: {
                        display: false
                    }
                }
            }
        }
    });

    
});