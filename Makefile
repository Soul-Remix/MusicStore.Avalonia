PROJECT_NAME = ToDoList
OUTPUT_DIR = bin
CLEAN_TARGETS = $(OUTPUT_DIR) obj

build:
	dotnet build

publish:
	dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:PublishTrimmed=true -p:TrimMode=copyused

clean:
	rm -rf $(CLEAN_TARGETS)

.PHONY: build clean publish

.PHONY: help
help:
	@echo "Available targets:"
	@echo "  build     : Build the Dotnet project."
	@echo "  publish     : Build the Dotnet project in release mode."
	@echo "  clean     : Remove built binaries."
