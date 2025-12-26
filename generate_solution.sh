rm -f yap.slnx
dotnet new solution --name yap -f slnx
FILES=$(find . -type f -name "*.csproj")
for FILE in $FILES; do
    echo "Adding $FILE to solution..."
    dotnet solution yap.slnx add $FILE
done
