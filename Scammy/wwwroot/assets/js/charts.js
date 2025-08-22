$(function () {
    'use strict'

    // Define fallback sample data for Flot charts
    var flotSampleData3 = [
        [0, 20], [10, 25], [20, 35], [30, 30], [40, 45], [50, 40], [60, 50], [70, 45], [80, 55], [90, 50], [100, 60]
    ];

    var flotSampleData4 = [
        [0, 15], [10, 20], [20, 25], [30, 35], [40, 30], [50, 35], [60, 40], [70, 35], [80, 45], [90, 40], [100, 50]
    ];

    var dashData2 = [
        [0, 10], [5, 15], [10, 20], [15, 25], [20, 30], [25, 25], [30, 35], [35, 30], [40, 25], [45, 20], [50, 15]
    ];

    // Check if real data exists, otherwise use sample data
    var flotData1, flotData2, smallChartData, barData1, barData2;

    if (window.realChartData && window.realChartData.mainChart && window.realChartData.mainChart.length > 0) {
        // Use real data
        flotData1 = window.realChartData.mainChart;
        flotData2 = window.realChartData.secondaryChart || window.realChartData.mainChart.map(d => [d[0], Math.max(0, d[1] - Math.floor(Math.random() * 3) - 1)]);

        // Create small chart data based on real data
        smallChartData = window.realChartData.mainChart.slice(-11).map((d, i) => [i * 5, d[1]]);

        // Bar chart data
        barData1 = window.realChartData.mainChart.slice(0, 8).map(d => d[1]);
        barData2 = flotData2.slice(0, 8).map(d => d[1]);

        // Ensure we have enough data points for bar chart
        while (barData1.length < 8) {
            barData1.push(Math.floor(Math.random() * 20) + 5);
        }
        while (barData2.length < 8) {
            barData2.push(Math.floor(Math.random() * 15) + 3);
        }
    } else {
        // Use sample data as fallback
        flotData1 = flotSampleData3;
        flotData2 = flotSampleData4;
        smallChartData = dashData2;
        barData1 = [2, 4, 10, 20, 45, 40, 35, 18];
        barData2 = [3, 6, 15, 35, 50, 45, 35, 25];
    }

    // Main Flot Chart
    var plot = $.plot('#flotChart', [{
        data: flotData1,
        color: '#007bff',
        lines: {
            fillColor: { colors: [{ opacity: 0 }, { opacity: 0.2 }] }
        }
    }, {
        data: flotData2,
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
            max: Math.max(...flotData1.map(d => d[1]), ...flotData2.map(d => d[1])) + 10,
            ticks: function (axis) {
                var max = axis.max;
                var step = Math.ceil(max / 4);
                return [[0, '0'], [step, step + 'K'], [step * 2, (step * 2) + 'K'], [step * 3, (step * 3) + 'K'], [max, max + 'K']];
            },
            tickColor: '#eee'
        },
        xaxis: {
            show: true,
            color: '#fff',
            ticks: function (axis) {
                if (window.realChartData && window.realChartData.mainChart && window.realChartData.mainChart.length > 0) {
                    // Use real date labels if available
                    var data = window.realChartData.mainChart;
                    var ticks = [];
                    for (var i = 0; i < Math.min(4, data.length); i++) {
                        var index = Math.floor(i * data.length / 4);
                        ticks.push([data[index][0], 'Day ' + (i + 1)]);
                    }
                    return ticks;
                } else {
                    // Use default labels
                    return [[25, 'OCT 21'], [75, 'OCT 22'], [100, 'OCT 23'], [125, 'OCT 24']];
                }
            }
        }
    });

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
            max: Math.max(...smallChartData.map(d => d[0])) + 5
        }
    });

    // Small Flot Chart 2 (Bar Chart)
    $.plot('#flotChart2', [{
        data: smallChartData,
        color: '#007bff'
    }], {
        series: {
            shadowSize: 0,
            bars: {
                show: true,
                lineWidth: 0,
                fill: 1,
                barWidth: 2
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
            max: Math.max(...smallChartData.map(d => d[0])) + 5
        }
    });

    // Initialize Peity charts if they exist
    if (typeof $.fn.peity !== 'undefined') {
        $('.peity-line').peity('line');
        $('.peity-bar').peity('bar');
        $('.peity-donut').peity('donut');
    }

    // Chart.js Bar Chart
    var ctx5 = document.getElementById('chartBar5');
    if (ctx5) {
        ctx5 = ctx5.getContext('2d');
        new Chart(ctx5, {
            type: 'bar',
            data: {
                labels: [0, 1, 2, 3, 4, 5, 6, 7],
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
                        max: Math.max(...barData1, ...barData2) + 10
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
    }

    // Optional: Add real-time data update function
    function updateCharts() {
        if (window.realChartData) {
            // Update main chart
            plot.setData([{
                data: window.realChartData.mainChart,
                color: '#007bff',
                lines: { fillColor: { colors: [{ opacity: 0 }, { opacity: 0.2 }] } }
            }, {
                data: window.realChartData.secondaryChart || window.realChartData.mainChart.map(d => [d[0], Math.max(0, d[1] - Math.floor(Math.random() * 3))]),
                color: '#560bd0',
                lines: { fillColor: { colors: [{ opacity: 0 }, { opacity: 0.2 }] } }
            }]);
            plot.draw();
        }
    }

    // Make update function globally available
    window.updateDashboardCharts = updateCharts;

    // Debug logging (remove in production)
    console.log('Charts initialized with data:', {
        hasRealData: !!(window.realChartData && window.realChartData.mainChart),
        mainChartPoints: flotData1.length,
        secondaryChartPoints: flotData2.length,
        barChartData1: barData1,
        barChartData2: barData2
    });

});