# GeoJsonFeatures
Get GeoJson feature of a coordinate with the help of OpenStreetMapApi

The application can make api calls for retrieving the features in a bounding box from http://openstreetmap.org/api/0.6 api. It gives me an osm formatted file. 
The convertion is made by osmtogeojson.js (https://github.com/tyrasd/osmtogeojson) library. It can be parsed into GeoJsonFeatures list with the help of this
little helper js. I made the openstreetmap api call from my WebApi project. The WebApi calls from the Web UI of the solution could have been made directly but
I prefered to call with HttpClient. Some problem that I have determined in the openstreetmap api caused not to set me up the edge case values of min_lon, min_lat,
max_lon and max_lat values. The api returns the 'The parameter bbox is required, and must be of the form min_lon,min_lat,max_lon,max_lat.' message when I make the 
'http://openstreetmap.org/api/0.6/map' request so I need to add parameters in 'http://openstreetmap.org/api/0.6/map?bbox=20,10,10,90' this format. According to
first message, the parameters should 'min_lon: 20, min_lat: 10, max_lon: 10, max_lon: 90' in second request; but when I send the request, It sends me back 
'The latitudes must be between -90 and 90, longitudes between -180 and 180 and the minima must be less than the maxima.' error. My brain confused a little bit
in this structure.
