# plywood-violin

## Proxies

Any proxies defined for Plywoon Violin will be ignored. This is an unintented consequence of the route re-ordering that occurs. 

The route re-rodering code clears all exisitng routes in preperation to applying the re-ordered routes, which also discards any proxy routes. As of this writing there was no obvious method of obtaining a list of proxy routes in code. 