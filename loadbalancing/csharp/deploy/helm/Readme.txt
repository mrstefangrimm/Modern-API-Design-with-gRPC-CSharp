helm create greet-server

helm template .

minikube start
helm install greet-server ./greet-server --dry-run
helm install greet-client ./greet-client --dry-run

helm dependency update ./umbrella

helm install my-release ./umbrella -n my-namespace --create-namespace
helm upgrade my-release ./umbrella -n my-namespace



helm list -n my-namespace
helm uninstall my-release -n my-namespace