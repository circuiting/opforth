\ opforth.fs

\ Copyright © 2025 Carlton Himes

\ This file is part of Opforth.

\ Opforth is free software: you can redistribute it and/or
\ modify it under the terms of the GNU General Public Li-
\ cense as published by the Free Software Foundation, either
\ version 3 of the License, or (at your option) any later
\ version.

\ Opforth is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warran-
\ ty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
\ See the GNU General Public License for more details.

\ You should have received a copy of the GNU General Public
\ License along with Opforth. If not, see
\ <https://www.gnu.org/licenses/>.


\ Contents


\ Core Stack (13)

\ drop     x --
\ dup      x -- x x
\ swap     x1 x2 -- x2 x1
\ over     x1 x2 -- x1 x2 x1
\ rot      x1 x2 x3 -- x2 x3 x1
\ ?dup     x -- x x | 0
\ 2drop    x1 x2 --
\ 2dup     x1 x2 -- x1 x2 x1 x2
\ 2swap    x1 x2 x3 x4 -- x3 x4 x1 x2
\ 2over    x1 x2 x3 x4 -- x1 x2 x3 x4 x1 x2
\ >r       exec: x R: -- R:x
\ r>       exec: R:x -- x R:
\ r@       exec: R:x -- x R:x


\ Core Extension Stack (7)

\ nip     x1 x2 -- x2
\ tuck    x1 x2 -- x2 x1 x2
\ pick    xu...x1 x0 u -- xu...x1 x0 xu
\ roll    xu xu-1...x0 u -- xu-1...x0 xu
\ 2>r     exec: x1 x2 R: -- R:x1 R:x2
\ 2r>     exec: R:x1 R:x2 -- x1 x2 R:
\ 2r@     exec: R:x1 R:x2 -- x1 x2 R:x1 R:x2


\ Core Arithmetic (18)

\ +         nu1 nu2 -- nu3
\ -         nu1 nu2 -- nu3
\ 1+        nu1 -- nu2
\ 1-        nu1 -- nu2
\ negate    n1 -- n2
\ abs       n -- u
\ s>d       n -- d
\ *         nu1 nu2 -- nu3
\ m*        n1 n2 -- d
\ um*       u1 u2 -- flag
\ /         n1 n2 -- n3
\ mod       n1 n2 -- n3
\ /mod      n1 n2 -- n3 n4
\ sm/rem    d1 n1 -- n2 n3
\ fm/mod    d1 n1 -- n2 n3
\ um/mod    d1 n1 -- n2 n3
\ */        n1 n2 n3 -- n4
\ */mod     n1 n2 n3 -- n4 n5


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


\ Core Stack Words


$0000 opcode drop  ( x -- )

\ Remove the top stack item.


$0001 opcode dup  ( x -- x x )

\ Duplicate the top stack item.


$0002 opcode swap  ( x1 x2 -- x2 x1 )

\ Exchange the top two stack items.


$0003 opcode over  ( x1 x2 -- x1 x2 x1 )

\ Put a copy of the second stack item on top of the stack.


$0004 opcode rot  ( x1 x2 x3 -- x2 x3 x1 )

\ Rotate the top three stack items to bring the third item to
\ the top.


$0005 opcode ?dup  ( x -- x x | 0 )

\ Duplicate the top stack item if it is nonzero.


: 2drop  ( x1 x2 -- )  drop drop ;

\ Remove the top two stack items.


: 2dup  ( x1 x2 -- x1 x2 x1 x2 )  over over ;

\ Duplicate the cell pair on top of the stack.


: 2swap  ( x1 x2 x3 x4 -- x3 x4 x1 x2 )  rot >r rot r> ;

\ Exchange the top two cell pairs on the stack.


: 2over  ( x1 x2 x3 x4 -- x1 x2 x3 x4 x1 x2 )
  2>r 2dup 2r> 2swap ;

\ Put a copy of cell pair x1 x2 on top of the stack.


$0006 opcode >r  ( exec: x R: -- R:x )

\ Interpretation: Undefined
\ Execution: Transfer the top data stack item to the return
\ stack.


$0007 opcode r>  ( exec: R:x -- x R: )

\ Interpretation: Undefined
\ Execution: Transfer the top return stack item to the data
\ stack.


$0008 opcode r@  ( exec: R:x -- x R:x )

\ Interpretation: Undefined
\ Execution: Put a copy of the top return stack item on the data
\ stack.


\ Core Extension Stack Words


$0009 opcode nip  ( x1 x2 -- x2 )

\ Remove the second stack item.


$000a opcode tuck  ( x1 x2 -- x2 x1 x2 )

\ Insert a copy of the top stack item underneath the second
\ stack item.


: pick  ( xu...x1 x0 u -- xu...x1 x0 xu )  sp@ + 1+ @ ;

\ Remove u and put a copy of xu, the stack item indexed by u, on
\ top of the stack. An ambiguous condition exists if there are
\ fewer than u+2 items on the stack before PICK is executed.


: roll  ( xu xu-1...x0 u -- xu-1...x0 xu )
  dup if swap >r 1- recurse r> swap exit then drop ;

\ Remove u and rotate the top u+1 stack items to bring xu to the
\ top.


: 2>r  ( exec: x1 x2 R: -- R:x1 R:x2 )  swap >r >r ;

\ Interpretation: Undefined
\ Execution: Transfer the cell pair on top of the data stack to
\ the return stack.


: 2r>  ( exec: R:x1 R:x2 -- x1 x2 R: )  r> r> swap ;

\ Interpretation: Undefined
\ Execution: Transfer the cell pair on top of the return stack
\ to the data stack.


: 2r@  ( exec: R:x1 R:x2 -- x1 x2 R:x1 R:x2 )
  r> r> 2dup >r >r swap ;

\ Interpretation: Undefined
\ Execution: Put a copy of the cell pair on top of the return
\ stack on the data stack.


\ Core Arithmetic Words


$000b opcode +  ( nu1 nu2 -- nu3 )

\ Add nu1 to nu2. nu3 is the sum.


$000c opcode -  ( nu1 nu2 -- nu3 )

\ Subtract nu2 from nu1. nu3 is the difference.


$000d opcode 1+  ( nu1 -- nu2 )

\ nu2 is nu1 incremented by one.


$000e opcode 1-  ( nu1 -- nu2 )

\ nu2 is nu1 decremented by one.


$000f opcode negate  ( n1 -- n2 )

\ n2 is the arithmetic inverse of n1.


$0010 opcode abs  ( n -- u )

\ u is the absolute value of n.


$0011 opcode s>d  ( n -- d )

\ Convert the single-cell number n to a double-cell number with
\ the same value. d is the result.
