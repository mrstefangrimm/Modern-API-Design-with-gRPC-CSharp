Install ArgoCD Operator on OpenShift
1. Install Operator: Red Hat OpenShift GitOp
2.a In Operator, create ArgoCD according to instructions
      https://docs.redhat.com/en/documentation/red_hat_openshift_gitops/1.19/html/argo_cd_instance/setting-up-argocd-instance
2.b In the AgroCD Yaml, update
  rbac:
    defaultPolicy: 'role:admin'  # Grants broader permissions
    policy: |
      p, role:admin, *, *, *, allow
      p, role:readonly, applications, get, */*, allow
    scopes: '[groups]'
3.a In the project openshift-gitops, route: https://openshift-gitops-server-openshift-gitops.apps-crc.testing
3.b Log in to ArgoCD with "Log in via OpenShift"
4. oc apply -f argocd/argocd-repo-rbac.yaml
5. oc apply -f argocd/argocd-appproject.yaml
6. oc apply -f argocd/argocd-app-greet-app.yaml

Verify
oc get applications -n openshift-gitops
oc get pods -n grpc-services

Test
oc port-forward greetclient-deploy-5b49655d5-n5m6h 8080:9091 -n grpc-services
curl -X POST -H 'Content-Type: application/json' --url 127.0.0.1:8080/greet --data '{"first_name": "Hitesh", "last_name":"Pattanayak"}'