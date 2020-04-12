require.config({
    shim: {
        'bootstrap': ['jquery'],
        'sparkline': ['jquery'],
        'tablesorter': ['jquery'],
        'vector-map': ['jquery'],
        'vector-map-de': ['vector-map', 'jquery'],
        'vector-map-world': ['vector-map', 'jquery'],
        'core': ['bootstrap', 'jquery'],
        'main': ['jquery', 'validate']
    },
    paths: {
        'main': 'assets/js/main',
        'core': 'assets/js/core',
        'jquery': 'assets/js/vendors/jquery-3.2.1.min',
        'bootstrap': 'assets/js/vendors/bootstrap.bundle.min',
        'sparkline': 'assets/js/vendors/jquery.sparkline.min',
        'selectize': 'assets/js/vendors/selectize.min',
        'tablesorter': 'assets/js/vendors/jquery.tablesorter.min',
        'vector-map': 'assets/js/vendors/jquery-jvectormap-2.0.3.min',
        'vector-map-de': 'assets/js/vendors/jquery-jvectormap-de-merc',
        'vector-map-world': 'assets/js/vendors/jquery-jvectormap-world-mill',
        'circle-progress': 'assets/js/vendors/circle-progress.min',
        'validate': 'assets/js/vendors/validate.min'
    }
});
require(['core']);

