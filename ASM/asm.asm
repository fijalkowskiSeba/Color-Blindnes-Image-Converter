.data
;matrix converting rgb to lms
rgb_to_lms dd 17.8824, 43.5161, 4.1194, 3.4557, 27.1554, 3.8671f, 0.0300, 0.1843, 1.4671;              
;matrix converting lms to rgb
lms_to_rgb dd  0.0809,   -0.1305,    0.1167,-0.0102,  0.0540,     -0.1136 ,-0.0004,  -0.0041,    0.6935 ;
;matrix converting lms to lms with pronatopia          
lms_to_pronatopia dd  0.0,    2.0234,   -2.5258 ,0.0,    1.0,      0.0 ,0.0,    0.0,      1.0; 
;zero    
zero_val dd 0
           
.code
;function to convert rgb matrix to rgb matrix with pronatopia
rgbEdit proc

movd xmm5, [zero_val]               ;0 will be stored in xmm5 
    
movups xmm0, [RCX]                  ;rgb matrix to xmm0   

movups xmm1, [rgb_to_lms]           ;first row of rgb_to_lms goes to xmm1 
movups xmm2, [rgb_to_lms+12]        ;second row of rgb_to_lms goes to xmm1          
movups xmm3, [rgb_to_lms+24]        ;third row of rgb_to_lms goes to xmm1

mulps  xmm0, xmm1                   ;first row * rgb
haddps xmm0, xmm0                   ;
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm11, xmm0                  ;first sum goes to xmm11

movups xmm0, [RCX]                  ;rgb matrix to xmm0  
mulps  xmm0, xmm2                   ;second row * rgb
haddps xmm0, xmm0                   ;
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm12, xmm0                  ;second sum goes to xmm12

movups xmm0, [RCX]                  ;rgb matrix to xmm0
mulps  xmm0, xmm3                   ;third row * rgb
haddps xmm0, xmm0                   ;
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm13, xmm0                  ;third sum goes to xmm13
                                    ;we need to combine 3 sums in array
movd xmm0, [zero_val]               ;zero to xmm0
movlhps xmm0, xmm11                 ;xmm11 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte to right
movlhps xmm0, xmm12                 ;xmm12 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte to right
movlhps xmm0, xmm13                 ;xmm12 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 93h              ;roll one byte to left
mulss xmm0, xmm5                    ;zero to most unsignificant byte of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte right 
movups xmm6, xmm0                   ;move xmm0 to xmm6, in xmm6 is LMS matrix

movups xmm1, [lms_to_pronatopia]    ;first row of lms_to_pronatopia goes to xmm1
movups xmm2, [lms_to_pronatopia+12] ;second row of lms_to_pronatopia goes to xmm1             
movups xmm3, [lms_to_pronatopia+24] ;third row of lms_to_pronatopia goes to xmm1   

mulps  xmm0, xmm1                   ;first row * lms
haddps xmm0, xmm0                   ;
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm11, xmm0                  ;first sum goes to xmm11

movups xmm0, xmm6                   ;lms to xmm0
mulps  xmm0, xmm2                   ;second row * lms
haddps xmm0, xmm0                   ;
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm12, xmm0                  ;second sum goes to xmm12

movups xmm0, xmm6                   ;lms to xmm0
mulps  xmm0, xmm3                   ;third row * lms
haddps xmm0, xmm0                   ;
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm13, xmm0                  ;third sum goes to xmm13

movd xmm0, [zero_val]               ;zero to xmm0
movlhps xmm0, xmm11                 ;xmm11 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte to right
movlhps xmm0, xmm12                 ;xmm12 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte to right
movlhps xmm0, xmm13                 ;xmm12 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 93h              ;roll one byte to left
mulss xmm0, xmm5                    ;zero to most unsignificant byte of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte right 
movups xmm7, xmm0                   ;move xmm0 to xmm7, in xmm7 is LMS with protanopia

movups xmm1, [lms_to_rgb]           ;first row of lms_to_rgb goes to xmm1
movups xmm2, [lms_to_rgb+12]        ;second row of lms_to_rgb goes to xmm1   
movups xmm3, [lms_to_rgb+24]        ;third row of lms_to_rgb goes to xmm1

mulps  xmm0, xmm1                   ;first row * lms with protanopia
haddps xmm0, xmm0              
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm11, xmm0                  ;first sum goes to xmm11

movups xmm0, xmm7                   ;lms with protanopia to xmm0
mulps  xmm0, xmm2                   ;second row * lms with protanopia
haddps xmm0, xmm0                   ;
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm12, xmm0                  ;second sum goes to xmm12


movups xmm0, xmm7                   ;lms with protanopia to xmm0
mulps  xmm0, xmm3                   ;third row * lms with protanopia
haddps xmm0, xmm0                   ;
haddps xmm0, xmm0                   ;2x hadpps gives sum of 4 floats
movaps xmm13, xmm0                  ;third sum goes to xmm13

movd xmm0, [zero_val]               ;0 goes to xmm0
movlhps xmm0, xmm11                 ;xmm11 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte to right
movlhps xmm0, xmm12                 ;xmm12 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte to right
movlhps xmm0, xmm13                 ;xmm12 goes to 2 most significant bytes of xmm0
shufps xmm0, xmm0, 93h              ;roll one byte to left
mulss xmm0, xmm5                    ;zero to most unsignificant byte of xmm0
shufps xmm0, xmm0, 39h              ;roll one byte right, in xmm0 is now result rgb 


movups [RCX], XMM0                  ;result rgb goes back to memory

ret                                 ;bye bye ;*
rgbEdit endp
end