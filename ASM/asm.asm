
.data
;matrix converting rgb to lms
rgb_to_lms dd 17.8824, 43.5161, 4.1194, 3.4557, 27.1554, 3.8671f, 0.0300, 0.1843, 1.4671;              
;matrix converting lms to rgb
lms_to_rgb dd  0.0809,   -0.1305,    0.1167,-0.0102,  0.0540,     -0.1136 ,-0.0004,  -0.0041,    0.6935 ;
;matrix converting lms to lms with pronatopia          
lms_to_pronatopia dd  0.0,    2.0234,   -2.5258 ,0.0,    1.0,      0.0 ,0.0,    0.0,      1.0; 
    
zero_val dd 0
           
.code
rgbEdit proc


movd xmm5, [zero_val]

movups xmm0, [RCX]              ;rgb 3x1

movups xmm1, [rgb_to_lms]       ;pierwszy rzad rgb_to_lms 
movups xmm2, [rgb_to_lms+12]    ;drugi  rzad rgb_to_lms          
movups xmm3, [rgb_to_lms+24]    ;trzeci rzad rgb_to_lms

mulps  xmm0, xmm1               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to L
haddps xmm0, xmm0              ;dodawanie, wynik to L
movaps xmm11, xmm0

movups xmm0, [RCX]              ;rgb 3x1
mulps  xmm0, xmm2               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to M
haddps xmm0, xmm0              ;dodawanie, wynik to M
movaps xmm12, xmm0


movups xmm0, [RCX]              ;rgb 3x1
mulps  xmm0, xmm3               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to S
haddps xmm0, xmm0              ;dodawanie, wynik to S
movaps xmm13, xmm0

movd xmm0, [zero_val]
movlhps xmm0, xmm11
shufps xmm0, xmm0, 39h
movlhps xmm0, xmm12
shufps xmm0, xmm0, 39h
movlhps xmm0, xmm13
shufps xmm0, xmm0, 93h
mulss xmm0, xmm5
shufps xmm0, xmm0, 39h
movups xmm6, xmm0
;teraz w xmm6 jest macierz LMS

movups xmm1, [lms_to_pronatopia]       ;pierwszy rzad rgb_to_lms 
movups xmm2, [lms_to_pronatopia+12]    ;drugi  rzad rgb_to_lms          
movups xmm3, [lms_to_pronatopia+24]    ;trzeci rzad rgb_to_lms

mulps  xmm0, xmm1               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to L
haddps xmm0, xmm0              ;dodawanie, wynik to L
movaps xmm11, xmm0

movups xmm0, xmm6              ;rgb 3x1
mulps  xmm0, xmm2               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to M
haddps xmm0, xmm0              ;dodawanie, wynik to M
movaps xmm12, xmm0


movups xmm0, xmm6             ;rgb 3x1
mulps  xmm0, xmm3               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to S
haddps xmm0, xmm0              ;dodawanie, wynik to S
movaps xmm13, xmm0

movd xmm0, [zero_val]
movlhps xmm0, xmm11
shufps xmm0, xmm0, 39h
movlhps xmm0, xmm12
shufps xmm0, xmm0, 39h
movlhps xmm0, xmm13
shufps xmm0, xmm0, 93h
mulss xmm0, xmm5
shufps xmm0, xmm0, 39h
movups xmm7, xmm0
;teraz w xmm7 jest macierz LMS z pronatopia

movups xmm1, [lms_to_rgb]       ;pierwszy rzad rgb_to_lms 
movups xmm2, [lms_to_rgb+12]    ;drugi  rzad rgb_to_lms          
movups xmm3, [lms_to_rgb+24]    ;trzeci rzad rgb_to_lms

mulps  xmm0, xmm1               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to L
haddps xmm0, xmm0              ;dodawanie, wynik to L
movaps xmm11, xmm0

movups xmm0, xmm7              ;rgb 3x1
mulps  xmm0, xmm2               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to M
haddps xmm0, xmm0              ;dodawanie, wynik to M
movaps xmm12, xmm0


movups xmm0, xmm7              ;rgb 3x1
mulps  xmm0, xmm3               ;mnozenie
haddps xmm0, xmm0              ;dodawanie, wynik to S
haddps xmm0, xmm0              ;dodawanie, wynik to S
movaps xmm13, xmm0

movd xmm0, [zero_val]
movlhps xmm0, xmm11
shufps xmm0, xmm0, 39h
movlhps xmm0, xmm12
shufps xmm0, xmm0, 39h
movlhps xmm0, xmm13
shufps xmm0, xmm0, 93h
mulss xmm0, xmm5
shufps xmm0, xmm0, 39h


movups [RDX], XMM0
movups xmm0, [RAX]

ret
rgbEdit endp
end