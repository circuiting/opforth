\ opforth.fs

\ Copyright © 2025 Carlton Himes

\ This file is part of Opforth.

\ Opforth is free software: you can redistribute it and/or
\ modify it under the terms of the GNU General Public License
\ as published by the Free Software Foundation, either version
\ 3 of the License, or (at your option) any later version.

\ Opforth is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty
\ of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
\ the GNU General Public License for more details.

\ You should have received a copy of the GNU General Public
\ License along with Opforth. If not, see
\ <https://www.gnu.org/licenses/>.



\ Contents


\ Core Stack

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
\ >r       Exe: x R: -- R:x
\ r>       Exe: R:x -- x R:
\ r@       Exe: R:x -- x R:x


\ Core Extension Stack

\ nip     x1 x2 -- x2
\ tuck    x1 x2 -- x2 x1 x2
\ pick    xu...x1 x0 u -- xu...x1 x0 xu
\ roll    xu xu-1...x0 u -- xu-1...x0 xu
\ 2>r     Exe: x1 x2 R: -- R:x1 R:x2
\ 2r>     Exe: R:x1 R:x2 -- x1 x2 R:
\ 2r@     Exe: R:x1 R:x2 -- x1 x2 R:x1 R:x2


\ Core Arithmetic

\ +         nu1 nu2 -- nu3
\ -         nu1 nu2 -- nu3
\ 1+        nu1 -- nu2
\ 1-        nu1 -- nu2
\ negate    n1 -- n2
\ abs       n -- u
\ s>d       n -- d
\ *         nu1 nu2 -- nu3
\ m*        n1 n2 -- d
\ um*       u1 u2 -- ud
\ /         n1 n2 -- n3
\ mod       n1 n2 -- n3
\ /mod      n1 n2 -- n3 n4
\ sm/rem    d n1 -- n2 n3
\ fm/mod    d n1 -- n2 n3
\ um/mod    d n1 -- n2 n3
\ */        n1 n2 n3 -- n4
\ */mod     n1 n2 n3 -- n4 n5


\ Core Number Test

\ 0=     x -- flag
\ 0<     n -- flag
\ =      x1 x2 -- flag
\ <      n1 n2 -- flag
\ >      n1 n2 -- flag
\ u<     u1 u2 -- flag
\ max    n1 n2 -- n3
\ min    n1 n2 -- n3


\ Core Extension Number Test

\ 0<>       x -- flag
\ 0>        n -- flag
\ <>        x1 x2 -- flag
\ u>        u1 u2 -- flag
\ within    nu1 nu2 nu3 -- flag


\ Core Bitwise Logic

\ invert    x1 -- x2
\ and       x1 x2 -- x3
\ or        x1 x2 -- x3
\ xor       x1 x2 -- x3
\ 2*        x1 -- x2
\ 2/        x1 -- x2
\ lshift    x1 u -- x2
\ rshift    x1 u -- x2


\ Core Extension Bitwise Logic

\ true     x -- true
\ false    x -- false


\ Core Address Math

\ cells      n1 -- n2
\ chars      n1 -- n2
\ cell+      a-addr1 -- a-addr2
\ char+      c-addr1 -- c-addr2
\ aligned    addr -- a-addr
\ count      c-addr1 -- c-addr2 u


\ Core Memory

\ @       a-addr -- x
\ !       x a-addr --
\ c@      c-addr -- x
\ c!      char c-addr --
\ 2@      a-addr -- x1 x2
\ 2!      x1 x2 a-addr --
\ +!      nu a-addr --
\ move    addr1 addr2 u --
\ fill    c-addr u char --


\ Core Extension Memory

\ erase    c-addr u -- false | i*x true
\ pad      -- c-addr


\ Core Text Display

\ ."        Inter,Compi: 'ccc"' --  Run: --
\ emit      x --
\ type      c-addr u --
\ cr        --
\ bl        -- char
\ space     --
\ spaces    n --


\ Core Extension Text Display

\ .(    Inter,Compi: 'ccc<close-paren>' --


\ Core Numeric String

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


\ Core Extension Numeric String

\ .r
\ u.r
\ holds
\ hex


\ Core Text Input

\ (
\ source
\ >in
\ key
\ accept
\ char
\ word


\ Core Extension Text Input

\ \
\ parse
\ parse-name
\ source-id
\ save-input
\ restore-input
\ refill


\ Core Execution Token

\ execute
\ '
\ find
\ >body


\ Core Extension Execution Token

\ defer@
\ defer!
\ action-of
\ is


\ Core Compiler

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


\ Core Extension Compiler

\ s\"
\ c"
\ compile,
\ [compile]


\ Core Definition

\ :
\ ;
\ immediate
\ constant
\ variable
\ create
\ does>


\ Core Extension Definition

\ :noname
\ buffer:
\ value
\ to
\ defer
\ marker


\ Core Control Flow

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


\ Core Extension Control Flow

\ again
\ ?do
\ case
\ of
\ endof
\ endcase


\ Core Query

\ depth
\ environment?


\ Core Extension Query

\ unused


\ Core Outer Interpreter

\ quit
\ abort
\ abort"
\ evaluate



\ Core Stack


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


$0006 opcode >r  ( Exe: x R: -- R:x )

\ Interpretation: Undefined
\ Execution: Transfer the top data stack item to the return
\ stack.


$0007 opcode r>  ( Exe: R:x -- x R: )

\ Interpretation: Undefined
\ Execution: Transfer the top return stack item to the data
\ stack.


$0008 opcode r@  ( Exe: R:x -- x R:x )

\ Interpretation: Undefined
\ Execution: Put a copy of the top return stack item onto the
\ data stack.



\ Core Extension Stack


$0009 opcode nip  ( x1 x2 -- x2 )

\ Remove the second stack item.


$000a opcode tuck  ( x1 x2 -- x2 x1 x2 )

\ Insert a copy of the top stack item under the second stack
\ item.


: pick  ( xu...x1 x0 u -- xu...x1 x0 xu )  sp@ + 1+ @ ;

\ Remove u and put a copy of xu, the stack item indexed by u, on
\ top of the stack. An ambiguous condition exists if there are
\ fewer than u+2 items on the stack before PICK is executed.


: roll  ( xu xu-1...x0 u -- xu-1...x0 xu )
  dup if swap >r 1- recurse r> swap exit then drop ;

\ Remove u and rotate the top u+1 stack items to bring xu to the
\ top.


: 2>r  ( Exe: x1 x2 R: -- R:x1 R:x2 )  swap >r >r ;

\ Interpretation: Undefined
\ Execution: Transfer the cell pair on top of the data stack to
\ the return stack.


: 2r>  ( Exe: R:x1 R:x2 -- x1 x2 R: )  r> r> swap ;

\ Interpretation: Undefined
\ Execution: Transfer the cell pair on top of the return stack
\ to the data stack.


: 2r@  ( Exe: R:x1 R:x2 -- x1 x2 R:x1 R:x2 )
  r> r> 2dup >r >r swap ;

\ Interpretation: Undefined
\ Execution: Put a copy of the cell pair on top of the return
\ stack onto the data stack.



\ Core Arithmetic


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

\ Convert the single-cell integer n to a double-cell integer
\ with the same value. d is the result.


: *  ( nu1 nu2 -- nu3 )  m* drop ;

\ Multiply nu1 by nu2. nu3 is the single-cell product.


: m*  ( n1 n2 -- d )  something ;

\ Multiply n1 by n2. d is the double-cell product.


: um*  ( u1 u2 -- ud )  something ;

\ Multiply u1 by u2. ud is the double-cell product. All values
\ and arithmetic are unsigned.


: /  ( n1 n2 -- n3 )  >r s>d r> sm/rem nip ;

\ Divide n1 by n2. n3 is the quotient. An ambiguous condition
\ exists if n2 is zero.


: mod  ( n1 n2 -- n3 )  >r s>d r> sm/rem drop ;

\ Divide n1 by n2. n3 is the remainder. If n1 and n2 differ in
\ sign, n3 is determined by symmetric division. An ambiguous
\ condition exists if n2 is zero.


: /mod  ( n1 n2 -- n3 n4 )  >r s>d r> sm/rem ;

\ Divide n1 by n2. n3 is the remainder and n4 is the quotient.
\ If n1 and n2 differ in sign, n3 and n4 are determined by sym-
\ metric division. An ambiguous condition exists if n2 is zero.


: sm/rem  ( d n1 -- n2 n3 )  something ;

\ Divide d by n1. n2 is the remainder and n3 is the quotient. If
\ d and n1 differ in sign, n2 and n3 are determined by symmetric
\ division. An ambiguous condition exists if n1 is zero or if n3
\ is outside the range of a single-cell signed integer.


: fm/mod  ( d n1 -- n2 n3 )  something ;

\ Divide d by n1. n2 is the remainder and n3 is the quotient. If
\ d and n1 differ in sign, n2 and n3 are determined by floored
\ division. An ambiguous condition exists if n1 is zero or if n3
\ is outside the range of a single-cell signed integer.


: um/mod  ( ud u1 -- u2 u3 )  something ;

\ Divide ud by u1. u2 is the remainder and u3 is the quotient.
\ All values and arithmetic are unsigned. An ambiguous condition
\ exists if u1 is zero or if u3 is outside the range of a
\ single-cell unsigned integer.


: */  ( n1 n2 n3 -- n4 )  >r m* r> sm/rem drop ;

\ Multiply n1 by n2 to produce an intermediate double-cell prod-
\ uct d, then divide d by n3. n4 is the quotient. If d and n3
\ differ in sign, n4 is determined by symmetric division. An am-
\ biguous condition exists if n3 is zero or if n4 is outside the
\ range of a single-cell signed integer.


: */mod  ( n1 n2 n3 -- n4 n5 )  >r m* r> sm/rem ;

\ Multiply n1 by n2 to produce an intermediate double-cell prod-
\ uct d, then divide d by n3. n4 is the remainder and n3 is the
\ quotient. If d and n3 differ in sign, n4 and n5 are determined
\ by symmetric division. An ambiguous condition exists if n3 is
\ zero or if n5 is outside the range of a single-cell signed
\ integer.



\ Core Number Test


$0012 opcode 0=  ( x -- flag )

\ If all bits of x are zero, flag is true. Otherwise flag is false.


$0013 opcode 0<  ( n -- flag )

\ If n is less than zero, flag is true. Otherwise flag is false.


$0014 opcode =  ( x1 x2 -- flag )

\ If x1 is bit-for-bit the same as x2, flag is true. Otherwise
\ flag is false.


: <  ( n1 n2 -- flag )  something ;

\ If n1 is less than n2, flag is true. Otherwise flag is false.


: >  ( n1 n2 -- flag )  something ;

\ If n1 is greater than n2, flag is true. Otherwise flag is
\ false.


$0015 opcode u<  ( u1 u2 -- flag )

\ If u1 is less than u2, flag is true. Otherwise flag is false.


: within  ( nu1 nu2 nu3 -- flag )  something ;

\ Description goes here
