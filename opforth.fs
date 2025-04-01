\ Opforth Core and Core Extension Words


\ Core Stack (13)

\ drop
\ dup
\ swap
\ over
\ rot
\ ?dup
\ 2drop
\ 2dup
\ 2swap
\ 2over
\ >r
\ r>
\ r@


\ Core Extension Stack (7)

\ nip
\ tuck
\ pick
\ roll
\ 2>r
\ 2r>
\ 2r@


\ Core Arithmetic (18)

\ 1+
\ 1-
\ +
\ -
\ negate
\ abs
\ s>d
\ *
\ m*
\ um*
\ /
\ mod
\ /mod
\ fm/mod
\ sm/rem
\ um/mod
\ */
\ */mod


\ Core Number Test (8)

\ 0=
\ 0<
\ =
\ <
\ >
\ u<
\ max
\ min


\ Core Extension Number Test (5)

\ 0<>
\ 0>
\ <>
\ u>
\ within


\ Core Bitwise Logic (8)

\ invert
\ and
\ or
\ xor
\ 2*
\ 2/
\ lshift
\ rshift


\ Core Extension Bitwise Logic (2)

\ true
\ false


\ Core Address Math (6)

\ cells
\ chars
\ cell+
\ char+
\ aligned
\ count


\ Core Memory (9)

\ @
\ !
\ c@
\ c!
\ 2@
\ 2!
\ +!
\ fill
\ move


\ Core Extension Memory (2)

\ erase
\ pad


\ Core Text Display (7)

\ ."
\ emit
\ cr
\ bl
\ space
\ spaces
\ type


\ Core Extension Text Display (1)

\ .(


\ Core Numeric String (11)

\ .
\ u.
\ <#
\ #>
\ #
\ #s
\ hold
\ sign
\ >number
\ base
\ decimal


\ Core Extension Numeric String (4)

\ .r
\ u.r
\ holds
\ hex


\ Core Text Input (7)

\ (
\ source
\ >in
\ key
\ accept
\ char
\ word


\ Core Extension Text Input (7)

\ \
\ parse
\ parse-name
\ source-id
\ save-input
\ restore-input
\ refill


\ Core Execution Token (4)

\ execute
\ '
\ find
\ >body


\ Core Extension Execution Token (4)

\ defer@
\ defer!
\ action-of
\ is


\ Core Compiler (13)

\ ,
\ c,
\ allot
\ align
\ here
\ postpone
\ literal
\ s"
\ [char]
\ [']
\ [
\ ]
\ state


\ Core Extension Compiler (4)

\ s\"
\ c"
\ compile,
\ [compile]


\ Core Definition (7)

\ :
\ ;
\ immediate
\ constant
\ variable
\ create
\ does>


\ Core Extension Definition (6)

\ :noname
\ buffer:
\ value
\ to
\ defer
\ marker


\ Core Control Flow (16)

\ if
\ else
\ then
\ begin
\ until
\ while
\ repeat
\ exit
\ recurse
\ do
\ loop
\ +loop
\ i
\ j
\ leave
\ unloop


\ Core Extension Control Flow (6)

\ again
\ ?do
\ case
\ of
\ endof
\ endcase


\ Core Query (2)

\ depth
\ environment?


\ Core Extension Query (1)

\ unused


\ Core Outer Interpreter (4)

\ quit
\ abort
\ abort"
\ evaluate
