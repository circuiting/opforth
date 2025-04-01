\ Opforth Words


\ Core Stack (14)
\
\ 2drop
\ 2dup
\ 2over
\ 2swap
\ >r
\ ?dup
\ depth
\ drop
\ dup
\ over
\ r>
\ r@
\ rot
\ swap


\ Core Extension Stack (8)
\
\ 2>r
\ 2r>
\ 2r@
\ nip
\ pick
\ roll
\ tuck
\ unused


\ Core Arithmetic (17)
\
\ *
\ */
\ */mod
\ +
\ -
\ /
\ /mod
\ 1+
\ 1-
\ abs
\ fm/mod
\ m*
\ mod
\ s>d
\ sm/rem
\ um*
\ um/mod


\ Core Number Test (8)
\
\ 0<
\ 0=
\ <
\ =
\ >
\ max
\ min
\ u<


\ Core Extension Number Test (5)
\
\ 0<>
\ 0>
\ <>
\ u>
\ within


\ Core Bitwise Logic (9)
\
\ 2*
\ 2/
\ and
\ invert
\ lshift
\ negate
\ or
\ rshift
\ xor


\ Core Extension Bitwise Logic (2)
\
\ false
\ true


\ Core Address Math (6)
\
\ aligned
\ cell+
\ cells
\ char+
\ chars
\ count


\ Core Memory (9)
\
\ !
\ +!
\ 2!
\ 2@
\ @
\ c!
\ c@
\ fill
\ move


\ Core Extension Memory (1)
\
\ erase


\ Core Text Display (8)
\
\ ."
\ bl
\ cr
\ emit
\ s"
\ space
\ spaces
\ type


\ Core Extension Text Display (3)
\
\ .(
\ c"
\ s\"


\ Core Numeric String (11)
\
\ #
\ #s
\ #>
\ .
\ <#
\ >number
\ base
\ decimal
\ hold
\ sign
\ u.


\ Core Extension Numeric String (4)
\
\ .r
\ hex
\ holds
\ u.r


\ Core Text Input (7)
\
\ (
\ >in
\ accept
\ char
\ key
\ source
\ word


\ Core Extension Text Input (7)
\
\ parse
\ parse-name
\ refill
\ restore-input
\ save-input
\ source-id
\ \


\ Core Execution Token (3)
\
\ '
\ execute
\ find


\ Core Extension Execution Token (4)
\
\ action-of
\ defer!
\ defer@
\ is


\ Core Compiler (13)
\
\ ,
\ >body
\ align
\ allot
\ c,
\ here
\ literal
\ postpone
\ state
\ [
\ [']
\ [char]
\ ]


\ Core Extension Compiler (2)
\
\ compile,
\ [compile]


\ Core Definition (7)
\
\ :
\ ;
\ constant
\ create
\ does>
\ immediate
\ variable


\ Core Extension Definition (6)
\
\ :noname
\ buffer:
\ defer
\ marker
\ to
\ value


\ Core Control Flow (16)
\
\ +loop
\ begin
\ do
\ else
\ exit
\ i
\ if
\ j
\ leave
\ loop
\ recurse
\ repeat
\ then
\ unloop
\ until
\ while


\ Core Extension Control Flow (6)
\
\ ?do
\ again
\ case
\ endcase
\ endof
\ of


\ Core Forth System (5)
\
\ abort
\ abort"
\ environment?
\ evaluate
\ quit


\ Core Extension Forth System (1)
\
\ pad
