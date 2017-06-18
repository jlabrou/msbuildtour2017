# Set up PowerShell variables to match those used in Azure Container Registries
$ACR_NAME = "jlvs6containers"
$ACR_PASS = "+=/+xa=y1L=+69I87h/8RPbiE+/s6Job"

docker login -u $ACR_NAME -p $ACR_PASS "$ACR_NAME.azurecr.io"

pause

# Add additional tags to the existing images
docker tag microsoft.knowzy.ordersapi "$ACR_NAME.azurecr.io/knowzy/ordersapi:1"
docker tag microsoft.knowzy.webapp "$ACR_NAME.azurecr.io/knowzy/webapp:1"

# View the new tags. Notice there are multiple tags for the same image id
docker image ls

pause

# The first push will take a moment
docker push "$ACR_NAME.azurecr.io/knowzy/ordersapi:1"

# The subsequent images will be much faster because they use a common base image
docker push "$ACR_NAME.azurecr.io/knowzy/webapp:1"

# Use the Cloud Shell to inspect your registry
#az acr repository list -n $ACR_NAME
#az acr repository show-tags -n $ACR_NAME --repository knowzy/ordersapi
#az acr repository show-tags -n $ACR_NAME --repository knowzy/webapp
