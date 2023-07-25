#!/bin/bash

while true; do
    clear
    echo -e "\e[34m Select a number \e[0m"
    echo 
    echo " 1-run"
    echo " 2-report"
    echo " 3-slides"
    echo " 4-show_report"
    echo " 5-show_slides"
    echo " 6-clean"
    echo " 7-quit" 

run() {
    echo -e "\e[34m Run Moogle \e[0m"
    cd ..
    cd ..
    cd Moogle-main--main
    dotnet watch run --project MoogleServer
    echo "Press Enter to continue" 
    read
}

report() {
    echo -e "\e[34m Running Report \e[0m"
    cd .. 
    cd Informe
    latexmk -pdf Informe.tex
    echo "Press Enter to continue"
    read
}

slides() {
    echo -e "\e[34m Running Slide \e[0m"
    cd ..
    cd Presentacion
    latexmk -pdf Presentacion.tex
    echo "Press Enter to continue"
    read
}

show_report() {
    echo -e "\e[34m Showing Report \e[0m"
    cd ..
    cd Informe
    xdg-open Informe.pdf
    echo "Press Enter to continue"
    read
}

show_slides() {
    echo -e "\e[34m Showing Slides \e[0m"
    cd ..
    cd Presentacion
    xdg-open Presentacion.pdf
    echo "Press Enter to continue"
    read
}
clean() {
    echo -e "\e[34m Cleaning files \e[0m"
    cd ..
    cd Informe
    rm Informe.aux Informe.fdb_latexmk Informe.fls Informe.log Informe.pdf Informe.toc
    cd ..
    cd Presentacion
    rm Presentacion.aux Presentacion.fdb_latexmk Presentacion.fls Presentacion.log Presentacion.nav Presentacion.snm Presentacion.vrb Presentacion.toc Presentacion.pdf
    cd sections 
    rm basics.aux intro.aux structure.aux conclusion.aux
    echo "Press Enter to continue"
    read
}
read option
    case $option in
        "1")
            run
            ;;
        "2")
            report
            ;;
        "3")
            slides
            ;;
        "4")
            show_report "report.pdf" 
            ;;
        "5")
            show_slides "slides.pdf" 
            ;;
        "6")
            clean
            ;;
        "7")
            echo -e "\e[34m Closing \e[0m"
            break
            ;;
    esac
done