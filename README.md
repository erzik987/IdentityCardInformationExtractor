# IdentityCardInformationExtractor


## Supervisor

* **Tomáš Caha**

## Authors

* **Erik Hudcovský** - *Initial work* - [PurpleBooth](https://github.com/erzik987)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Build 

To realy use this program, you have to download data witch OCR(Tesseract) uses for character recognizion. The data can be found [here](https://github.com/tesseract-ocr/tessdata). Simply choose data witch conrespond with language on your ID card. Now create folder in directory "netcoreapp3.0" (.\IdentityCardInformationExtractor\bin\Debug\netcoreapp3.0) and name it "tessdata" (name can be easily changed). Copy the trained data here and in the class TessData.cs change language to the one you chose, here you can also change the folder where trained data should be stored.
