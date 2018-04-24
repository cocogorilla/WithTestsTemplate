So, really annoying, but to get past security restrictions for local development testing of the integration tests
you are going to have to maniuplate your system to grant permissions to bind to something local.

Now... you could just use "localhost".  Microsoft has done some magic that allows this to pass, mainly by not actually
binding to your localhost or issueing any requests.  It just sort of in-memory loopbacks and spoofs it, thereby dodging
security.

Problem is... when you hookup fiddler, you'll see nothing.

I want to actually see the request traces being run in fiddler.  So:

Decide on a name like:
mylocal

Modify hosts file:
127.0.0.1   mylocal

Now either run visual studio as administrator (which sucks)
or
Run the following commands in a command line window executed under administrator

netsh http add urlacl url=http://mylocal:8080/ user=CHURCHILL\Ethan
netsh http add urlacl url=http://mylocal:8081/ user=CHURCHILL\Ethan
netsh http add urlacl url=http://mylocal:8082/ user=CHURCHILL\Ethan
netsh http add urlacl url=http://mylocal:8083/ user=CHURCHILL\Ethan

If you need to generate a certificate separate than the test certificate stored with this project:
This command must be run in powershell (comes with windows 10) running as administrator
new-selfsignedcertificate -CertStoreLocation cert:\LocalMachine\My -Subject mylocal -provider "Microsoft Enhanced RSA and AES Cryptographic Provider"
Modify config files with the name of your new cert (relative filepaths are preferred so other's can use this project)
Additionally, make sure the visual studio properties for the certificate file are set to "copy newer" or "copy always"