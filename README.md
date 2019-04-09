#Introduction:
With this control, I try to solve a problem common to all of us who do not have English as our native language. All led, lcd, seven segments controls I have seen so far do not support more than the 7 bit ASCII character set. So for all of us that would like to have our own specific character or symbol, I have added a character editor. I was inspired by another led control by Liu Xia A Fine-looking Segmented LED Control from where I also borrowed a few things like ISupportInitialize.

#Background: 
There are some quite nice, free led fonts on the Internet like Ozone by Andreas Nylin and AI pointe by Ritchie Ned Hansel. Al pointe uses a 5x7 matrix, a nice light font, and Ozone uses a bolder 6x8 matrix. Have a look and get some inspiration to create your own font. Feel free to use anything between 4x4 (never tried) and 8x8, 5x7, 6x10 or anything that multiplies to 64 or less.

#Using the code:
This control has just a few specific public properties, two to change the appearance of the font, and three for scrolling. Besides that, you can of course change ForeColor and BackColor, which also support transparency. The font you create is stored in a Dictionary key=char, value=bits and saved to an XML file in your Documents directory using:
Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LedFont.xml";
This is because the file is needed in design mode as well as in run time, explained in Points of Interest. The file created contains the characters and their binary matrix as a ulong, e.g. character char="A" bits="227873781661662" which translates to “0000 0000 0000 0000 1100 1111 0011 1111 1111 1111 1100 1111 0011 1111 1101 1110” in binary.
If you read from the right, you can match bit by bit in the editor starting from the upper left corner. 
It is also possible to change the layout of the matrix, currently 6x8, by changing the constants HSEGMENTS and VSEGMENTS. 
But be sure to make a backup of your previous font file before that and start with a new one, or else if the matrix doesn't comply with your font, it will be scrambled. 
As the font isn't a bitmap, it's freely resizable and so far I have tested a 16 pixel high and up to 120 pixel high font. Of course the bigger it is, the higher will be the CPU load.
I have included a LedFont.xml with capital letters and numbers if you would like to get started. 
Just remember to copy this file to your documents folder.


