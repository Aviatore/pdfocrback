if [[ $HOST_IP != "" ]]
then
    cd /usr/share/nginx/html
    echo "Changing localhost to $HOST_IP ..."
    sed -i "s/localhost/$HOST_IP/g" main.js
fi

if [[ $MAX_FILE_SIZE != "" ]]
then
    cd /etc/nginx
    sed -i "s/client_max_body_size 1M;/client_max_body_size ${MAX_FILE_SIZE};/" nginx.conf
fi

service nginx start

cd /App
dotnet PdfOcr.dll
