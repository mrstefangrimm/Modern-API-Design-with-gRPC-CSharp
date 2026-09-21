helm create greet-server

helm template .

minikube start
helm install greet-server ./greet-server --dry-run
helm install greet-client ./greet-client --dry-run

helm dependency update ./umbrella

helm install my-release ./umbrella -n my-namespace --create-namespace
helm upgrade --install my-release ./umbrella -n my-namespace --create-namespace


Test no load balancing
kubectl port-forward service/my-release-cnlb-greet-client 80:9091 -n my-namespace

Test static load balancing
kubectl port-forward service/my-release-cslb-greet-client 80:9092 -n my-namespace

Test dynamic load balancing
kubectl port-forward service/my-release-cdlb-greet-client 80:9094 -n my-namespace


curl 127.0.0.1
curl -X POST -H 'Content-Type: application/json' --url 127.0.0.1/greet --data '{"first_name": "Hitesh", "last_name":"Pattanayak"}'


helm list -n my-namespace
helm uninstall my-release -n my-namespace