PASSWORD=grpc
if [ $# -ge 1 ]
  then
    PASSWORD=$1
fi

cat <<EOT >>client.config
[ req ]
default_bits       = 2048
default_md         = sha256
default_keyfile    = client.key
prompt             = no
encrypt_key        = no

distinguished_name = dn
req_extensions     = v3_req
x509_extensions    = x509_req
string_mask        = utf8only

[ dn ]
commonName             = localhost client cert
emailAddress           = test@localtest.me
countryName            = US
stateOrProvinceName    = DE
localityName           = Wilmington
organizationName       = Todo World

[ x509_req ]
subjectKeyIdentifier   = hash
authorityKeyIdentifier = keyid,issuer
basicConstraints       = critical, CA:false
keyUsage               = critical, keyEncipherment
subjectAltName         = @alt_names
# extendedKeyUsage     = serverAuth, clientAuth
nsComment              = "OpenSSL Generated Certificate"

[ v3_req ]
subjectKeyIdentifier   = hash
basicConstraints       = critical, CA:false
subjectAltName         = @alt_names
# extendedKeyUsage     = serverAuth, clientAuth
nsComment              = "OpenSSL Generated Certificate"

[ alt_names ]
DNS.1                  = localhost
EOT

openssl req -config client.config -new -out client.csr.pem
openssl x509 -req -days 365 -extfile client.config -extensions v3_req -in client.csr.pem -signkey client.key -out client.crt
openssl pkcs12 -export -out client.pfx -inkey client.key -in client.crt -password pass:$PASSWORD
rm client.config client.csr.pem
# cp client.pfx ../MyApp