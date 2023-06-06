# URL_shortening_Service

this project  is a simple shortening service also contains redirection to the previusly shorted urls

geting shorted url you need to call shorter controller with  url parameter .
url paramerter should be contain long url.

sample request:
https://localhost:44380/api/Shorter?url=https://www.trendyol.com/universal/2-bisiklet-tasiyici-2-bisiklet-tasima-aparati-p-124155298

Sample Response:
"https://localhost:44380/api/Shorter?sCode=GFSQRI"

when you clicked to the url which was given in response it will aoutomaticallay redirected to the saved long url.
and also users can chose their own short code with chosen code parameter sample request is down below

sample request:
https://localhost:44380/api/Shorter?url=https://www.trendyol.com/universal/2-bisiklet-tasiyici-2-bisiklet-tasima-aparati-p-124155298&chosenCode=alicom

Sample Response:
"https://localhost:44380/api/Shorter?sCode=alicom"

