rm -f yap.sln
dotnet new solution --name yap
FILES=$(find . -type f -name "*.csproj")
for FILE in $FILES; do
    echo "Adding $FILE to solution..."
    dotnet solution yap.sln add $FILE
done
