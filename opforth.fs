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

\ Comments in the source code are adapted from descriptions
\ published by the Forth Standard Committee. See
\ https://forth-standard.org



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
\ >r       Compi: --  Exe: x R: -- R:x
\ r>       Compi: --  Exe: R:x -- x R:
\ r@       Compi: --  Exe: R:x -- x R:x


\ Core-Ext Stack

\ nip     x1 x2 -- x2
\ tuck    x1 x2 -- x2 x1 x2
\ pick    xu...x1 x0 u -- xu...x1 x0 xu
\ roll    xu xu-1...x0 u -- xu-1...x0 xu
\ 2>r     Compi: --  Exe: x1 x2 R: -- R:x1 R:x2
\ 2r>     Compi: --  Exe: R:x1 R:x2 -- x1 x2 R:
\ 2r@     Compi: --  Exe: R:x1 R:x2 -- x1 x2 R:x1 R:x2


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
\ um/mod    ud u1 -- u2 u3
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


\ Core-Ext Number Test

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


\ Core-Ext Bitwise Logic

\ true     -- true
\ false    -- false


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
\ c@      c-addr -- char
\ c!      char c-addr --
\ 2@      a-addr -- x1 x2
\ 2!      x1 x2 a-addr --
\ +!      nu a-addr --
\ move    addr1 addr2 u --
\ fill    c-addr u char --


\ Core-Ext Memory

\ erase    addr u --
\ pad      -- c-addr


\ Core Text Display

\ ."        'ccc<quote>' --  Run: --
\ emit      x --
\ type      c-addr u --
\ cr        --
\ bl        -- char
\ space     --
\ spaces    n --


\ Core-Ext Text Display

\ .(    'ccc<right-paren>' --


\ Core Numeric String

\ .          n --
\ u.         u --
\ <#         --
\ #>         xd -- c-addr u
\ #          ud1 -- ud2
\ #s         ud1 -- ud2
\ hold       char --
\ sign       n --
\ decimal    --
\ base       -- a-addr
\ >number    ud1 c-addr1 u1 -- ud2 c-addr2 u2


\ Core-Ext Numeric String

\ .r       n1 n2 --
\ u.r      u n --
\ holds    c-addr u --
\ hex      --


\ Core Text Input

\ (         'ccc<right-paren>' --  Run: --
\ source    -- c-addr u
\ >in       -- a-addr
\ key       -- char
\ accept    c-addr +n1 -- +n2
\ char      '<spaces>name' -- char
\ word      '<chars>ccc<char>' char -- c-addr


\ Core-Ext Text Input

\ \                'ccc<eol>' --  Run: --
\ parse            'ccc<char>' char -- c-addr u
\ parse-name       '<spaces>name<space>' -- c-addr u
\ source-id        -- 0 | -1
\ save-input       -- xn...x1 n
\ restore-input    xn...x1 n -- flag
\ refill           -- flag


\ Core Query

\ depth           -- +n
\ environment?    c-addr u -- false | i*x true


\ Core-Ext Query

\ unused    -- u


\ Core Execution Token

\ execute    i*x xt -- j*x
\ '          '<spaces>name' -- xt
\ find       c-addr -- c-addr 0 | xt 1 | xt -1
\ >body      xt -- a-addr


\ Core-Ext Execution Token

\ defer@       xt1 -- xt2
\ defer!       xt2 xt1 --
\ is           Inter: '<spaces>name' xt --
\              Compi: '<spaces>name' --  Run: xt --
\ action-of    Inter: '<spaces>name' -- xt
\              Compi: '<spaces>name' --  Run: -- xt


\ Core Compiler

\ ,           x --
\ c,          char --
\ allot       n --
\ align       --
\ here        -- addr
\ [           --
\ ]           --
\ state       -- a-addr
\ postpone    Compi: '<spaces>name' --
\ literal     Compi: x --  Run: -- x
\ [char]      Compi: '<spaces>name' --  Run: -- char
\ [']         Compi: '<spaces>name' --  Run: -- xt
\ s"          Compi: 'ccc<quote>' --  Run: -- c-addr u


\ Core-Ext Compiler

\ s\"          Compi: 'ccc<quote>' --
\ c"           Compi: 'ccc<quote>' --  Run: -- c-addr
\ compile,     Compi: --  Exe: xt --
\ [compile]    Compi: '<spaces>name' --


\ Core Definition

\ :            '<spaces>name' -- colon-sys
\              Initi: i*x R: -- i*x R:nest-sys  Exe: i*x -- j*x
\ ;            Compi: colon-sys --  Run: R:nest-sys -- R:
\ immediate    --
\ constant     '<spaces>name' x --  Exe: -- x
\ variable     '<spaces>name' --  Exe: -- a-addr
\ create       '<spaces>name' --  Exe: -- a-addr
\ does>        Compi: colon-sys1 -- colon-sys2
\              Run: R:nest-sys1 -- R:
\              Initi: i*x R: -- i*x a-addr R:nest-sys2
\              Exe: i*x -- j*x


\ Core-Ext Definition

\ :noname    -- xt colon-sys
\            Initi: i*x R: -- i*x R:nest-sys  Exe: i*x -- j*x
\ buffer:    '<spaces>name' u --  Exe: -- a-addr
\ value      '<spaces>name' x --  Exe: -- x  to: x --
\ to         Inter: '<spaces>name' i*x --
\            Compi: '<spaces>name' --
\ defer      '<spaces>name' --  Exe: i*x -- j*x
\ marker     '<spaces>name' --  Exe: --


\ Core Control Flow

\ if         Compi: -- orig  Run: x --
\ else       Compi: orig1 -- orig2  Run: --
\ then       Compi: orig --  Run: --
\ begin      Compi: -- dest  Run: --
\ until      Compi: dest --  Run: x --
\ while      Compi: dest -- orig dest  Run: x --
\ repeat     Compi: orig dest --  Run: --
\ do         Compi: -- do-sys  Run: nu1 nu2 R: -- R:n
\ loop       Compi: do-sys --  Run: R:n1 -- R:|n2
\ +loop      Compi: do-sys --  Run: n1 R:n2 -- R:|n3
\ i          Compi: --  Exe: R:n -- nu R:n
\ j          Compi: --  Exe: R:n1 n2 -- nu n1 n2
\ leave      Compi: --  Exe: R:n -- R:
\ unloop     Compi: --  Exe: R:n -- R:
\ exit       Compi: --  Exe: R:nest-sys -- R:
\ recurse    Compi: --


\ Core-Ext Control Flow

\ again      Compi: dest --  Run: --
\ ?do        Compi: -- do-sys  Run: nu1 nu2 R: -- R:n
\ case       Compi: -- case-sys  Run: --
\ of         Compi: -- of-sys  Run: x1 x2 -- |x1
\ endof      Compi: case-sys1 of-sys -- case-sys2  Run: --
\ endcase    Compi: case-sys  Run: x --


\ Core Outer Interpreter

\ quit        R:i*x -- R:
\ abort       i*x R:j*x -- R:
\ abort"      Compi: 'ccc<quote>' --  Run: i*x x1 R:j*x -- |i*x R:|j*x
\ evaluate    i*x c-addr u -- j*x



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


$0006 opcode >r  ( Compi: -- ) ( Exe: x R: -- R:x )
compile-only

\ Interpretation: Undefined

\ Compilation: Compile the following execution semantics.

\ Execution: Transfer the top data stack item to the return
\ stack.


$0007 opcode r>  ( Compi: -- ) ( Exe: R:x -- x R: )
compile-only

\ Interpretation: Undefined

\ Compilation: Compile the following execution semantics.

\ Execution: Transfer the top return stack item to the data
\ stack.


$0008 opcode r@  ( Compi: -- ) ( Exe: R:x -- x R:x )
compile-only

\ Interpretation: Undefined

\ Compilation: Compile the following execution semantics.

\ Execution: Put a copy of the top return stack item onto the
\ data stack.



\ Core-Ext Stack


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


: 2>r  ( Compi: -- ) ( Exe: x1 x2 R: -- R:x1 R:x2 )
  postpone swap  postpone >r  postpone >r
; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the following execution semantics.

\ Execution: Transfer the cell pair on top of the data stack to
\ the return stack.


: 2r>  ( Compi: -- ) ( Exe: R:x1 R:x2 -- x1 x2 R: )
  postpone r>  postpone r>  postpone swap
; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the following execution semantics.

\ Execution: Transfer the cell pair on top of the return stack
\ to the data stack.


: 2r@  ( Compi: -- ) ( Exe: R:x1 R:x2 -- x1 x2 R:x1 R:x2 )
  postpone r>  postpone r>  postpone 2dup
  postpone >r  postpone >r  postpone swap
; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the following execution semantics.

\ Execution: Put a copy of the cell pair on top of the return
\ stack onto the data stack.



\ Core Arithmetic


$000b opcode +  ( nu1 nu2 -- nu3 )

\ Add nu1 to nu2. nu3 is the sum.


$000c opcode -  ( nu1 nu2 -- nu3 )

\ Subtract nu2 from nu1. nu3 is the difference.


$000d opcode 1+  ( nu1 -- nu2 )

\ nu2 is the result of incrementing nu1 by one.


$000e opcode 1-  ( nu1 -- nu2 )

\ nu2 is the result of decrementing nu1 by one.


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


: max  ( n1 n2 -- n3 )  something ;

\ Compare the top two integers on the stack. n3 is the integer
\ that is greater (closer to positive infinity).


: min  ( n1 n2 -- n3 )  something ;

\ Compare the top two integers on the stack. n3 is the integer
\ that is lesser (closer to negative infinity).



\ Core-Ext Number Test


$0016 opcode 0<>  ( x -- flag )

\ If all bits of x are zero, flag is false. Otherwise flag is
\ true.


$0017 opcode 0>  ( n -- flag )

\ If n is greater than zero, flag is true. Otherwise flag is
\ false.


$0018 opcode <>  ( x1 x2 -- flag )

\ If x1 is bit-for-bit the same as x2, flag is false. Otherwise
\ flag is true.


$0019 opcode u>  ( u1 u2 -- flag )

\ If u1 is greater than u2, flag is true. Otherwise flag is
\ false.


: within  ( nu1 nu2 nu3 -- flag )  something ;

\ Description of something goes here



\ Core Bitwise Logic


$001a opcode invert  ( x1 -- x2 )

\ x2 is the result of inverting all bits of x1.


$001b opcode and  ( x1 x2 -- x3 )

\ x3 is the bit-for-bit logical AND of x1 with x2.


$001c opcode or  ( x1 x2 -- x3 )

\ x3 is the bit-for-bit logical OR of x1 with x2.


$001d opcode xor  ( x1 x2 -- x3 )

\ x3 is the bit-for-bit logical XOR of x1 with x2.


$001c opcode 2*  ( x1 -- x2 )

\ x2 is the result of shifting all bits of x1 to the left by one
\ binary digit. The vacated least significant bit becomes zero.


$001d opcode 2/  ( x1 -- x2 )

\ x2 is the result of shifting all bits of x1 to the right by
\ one binary digit. The vacated most significant bit is un-
\ changed.


: lshift  ( x1 u -- x2 )  something ;

\ x2 is the result of shifting all bits of x1 to the left by u
\ binary digits. The vacated least significant bits become ze-
\ roes.


: rshift  ( x1 u -- x2 )  something ;

\ x2 is the result of shifting all bits of x1 to the right by u
\ binary digits. The vacated most significant bits are unchanged.



\ Core-Ext Bitwise Logic


$001e opcode true  ( -- true )

\ Put a logical TRUE flag on the stack. All bits of the flag are
\ ones.


$001f opcode false  ( -- false )

\ Put a logical FALSE flag on the stack. All bits of the flag
\ are zeroes.



\ Core Address Math


: cells  ( n1 -- n2 )  ; immediate

\ n2 is the size in address units of n1 cells. Because the size
\ of an Opforth cell is one address unit, n2 is equal to n1.


: chars  ( n1 -- n2 )  ; immediate

\ n2 is the size in address units of n1 character. Because the
\ size of an Opforth character is one address unit, n2 is equal
\ to n1.


synonym cell+ 1+  ( a-addr1 -- a-addr2 )

\ a-addr2 is the result of incrementing a-addr1 by one address
\ unit. Because the size of an Opforth cell is one address unit,
\ this operation is equivalent to 1+.


synonym char+ 1+  ( c-addr1 -- c-addr2 )

\ a-addr2 is the result of incrementing c-addr1 by one address
\ unit. Because the size of an Opforth character is one address
\ unit, this operation is equivalent to 1+.


: aligned  ( addr -- a-addr )  ; immediate

\ a-addr is the first aligned address greater than or equal to
\ addr. Because the size of an Opforth cell is one address unit,
\ a-addr is equal to addr.


: count  ( c-addr1 -- c-addr2 u )  something ;

\ Given a counted string located at c-addr1, return the non-
\ counted string representation. c-addr2 is the location of the
\ first character after the count character, and u is the string
\ length excluding the count character.



\ Core Memory


$0020 opcode @  ( a-addr -- x )

\ Read the cell located at memory address a-addr and put the
\ cell on the stack.


: !  ( x a-addr -- )  tuck! drop ;

\ Write x to memory address a-addr.


synonym c@ @  ( c-addr -- char )

\ Read the character located at memory address c-addr and put
\ the character on the stack.


synonym c! !  ( char c-addr -- )

\ Write char to memory address c-addr.


: 2@  ( a-addr -- x1 x2 )  dup cell+ @ swap @ ;

\ Read the cell pair located at memory address a-addr and put
\ the cell pair on the stack. x2 is the cell at a-addr and x1
\ is the next consecutive cell.


: 2!  ( x1 x2 a-addr -- )  tuck ! cell+ ! ;

\ Write the cell pair x1 x2 to memory address a-addr. x2 is
\ written to a-addr and x1 is written to the next consecutive
\ cell.


: +!  ( nu a-addr -- )  tuck @ + swap ! ;

\ Read the single-cell integer located at memory address a-addr,
\ add nu to the integer, and write the result to the same ad-
\ dress.


: move  ( addr1 addr2 u -- )  something ;

\ If u is greater than zero, write a copy of the u consecutive
\ address units of memory starting at addr1 to the u consecutive
\ address units starting at addr2. The u characters of memory
\ starting at addr2 will contain exactly what the u characters
\ of memory starting at addr1 contained before the move, even if
\ the memory regions overlap.


: fill  ( c-addr u char -- )  something ;

\ If u is greater than zero, write char to each of the u consec-
\ utive characters beginning at memory address c-addr. Because
\ Opforth characters and cells are the same size, this is equi-
\ valent to writing char to u consecutive cells.



\ Core-Ext Memory


: erase  ( addr u -- )  something ;

\ If u is greater than zero, clear all bits of the u consecutive
\ memory locations starting at address addr. Because the size of
\ an Opforth cell is one address unit, this is equivalent to
\ writing to u consecutive cells.


: pad  ( -- c-addr )  something ;

\ c-addr is the address of the pad, which is a region of memory
\ that can be used to hold data for intermediate processing.



\ Core Text Display


: ."  ( 'ccc<quote>' -- )  ( Run: -- )  something ;

\ Interpretation: Parse ccc delimited by " (double-quote). Dis-
\ play ccc.

\ Compilation: Parse ccc delimited by " (double-quote). Compile
\ the following runtime semantics.

\ Runtime: Display ccc.


: emit  ( x -- )  something ;

\ If x is a graphic character, display x.


: type  ( c-addr u -- )  something ;

\ If u is greater than zero, display the string with starting
\ address c-addr and length u.


: cr  ( -- )  something ;

\ Move the text cursor to the beginning of the next line.


: bl  ( -- char )  something ;

\ char is the code for the space character. Because Opforth uses
\ ASCII/UTF-8 and the size of an Opforth character is 16 bits,
\ char is $0020.


: space  ( -- )  something ;

\ Display one space.


: spaces  ( n -- )  something ;

\ If n is greater than zero, display n spaces.



\ Core-Ext Text Display


: .(  ( 'ccc<right-paren> -- )  something ; immediate

\ Parse ccc delimited by ) (right parenthesis). Display ccc.



\ Core Digit String

: .  ( n -- )  something ;

\ Display a text representation of the integer n followed by a
\ space.


: u.  ( u -- )  something ;

\ Display a text representation of the unsigned integer u fol-
\ lowed by a space.


: <#  ( -- )  something ;

\ Start a number-to-string conversion.


: #>  ( xd -- c-addr u )  something ;

\ Finish a number-to-string conversion by dropping xd and making
\ the string available. c-addr is the starting address of the
\ string, and u is the string length. A program may replace
\ characters within the string.


: #  ( ud1 -- ud2 )  something ;

\ As part of a <# #> delimited number-to-string conversion, con-
\ vert one digit of ud1 using the following method. Divide ud1
\ by the number in BASE. Prepend a text representation of the
\ remainder to the string being built. ud2 is the quotient,
\ which can be used by a subsequent number-to-string conversion
\ word. An ambiguous condition exists if # is executed outside
\ of a <# #> delimited conversion.


: #s  ( ud1 -- ud2 )  something ;

\ As part of a <# #> delimited number-to-string conversion, con-
\ vert all digits of ud1 and prepend the digits to the string
\ being built. ud2 is zero. An ambiguous condition exists if #S
\ is executed outside of a <# #> delimited conversion.


: hold  ( char -- )  something ;

\ As part of a <# #> delimited number-to-string conversion, pre-
\ pend char to the string being built. An ambiguous condition
\ exists if HOLD is executed outside of a <# #> delimited con-
\ version.


: sign  ( n -- )  something ;

\ As part of a <# #> delimited number-to-string conversion, put
\ a minus sign at the beginning of the string being built if n
\ is negative. An ambiguous condition exists if SIGN is executed
\ outside of a <# #> delimited conversion.


: decimal  ( -- )  something ;

\ Set the base (radix) of the number system to ten.


variable base  ( -- a-addr )  #10 base !

\ a-addr is the address of a cell containing the base (radix) of
\ the number system.


: >number  ( ud1 c-addr1 u1 -- ud2 c-addr2 u2 )  something ;

\ Convert the string specified by c-addr1 u1 into a double-cell
\ unsigned integer using the following method. ( something )



\ Core-Ext Numeric String


: .r  ( n1 n2 -- )  something ;

\ Display a text representation of n1 right-aligned in a field
\ n2 characters wide. If the number of characters required to
\ display n1 is greater than n2, all digits are displayed with
\ no leading spaces in a field as wide as necessary.


: u.r  ( u n -- )  something ;

\ Display a text representation of u right-aligned in a field n
\ characters wide. If the number of characters required to dis-
\ play n1 is greater than n2, all digits are displayed with no
\ leading spaces in a field as wide as necessary.


: holds  ( c-addr u -- )  something ;

\ As part of a <# #> delimited number-to-string conversion, pre-
\ pend the string represented by c-addr u to the string being
\ built. An ambiguous condition exists if HOLDS is executed out-
\ side of a <# #> delimited numeric string conversion.


: hex  ( -- )  something ;

\ Set the base (radix) of the number system to 16 (hexadecimal).



\ Core Text Input


: (  ( 'ccc<right-paren>' -- ) ( Run: -- )
  something ; immediate

\ Parse ccc delimited by ) (right parenthesis). This causes the
\ outer interpreter to skip past the text enclosed in the paren-
\ theses.


: source  ( -- c-addr u )  something ;

\ c-addr is the address of the input buffer. u is the number of
\ characters in the input buffer.


variable >in  ( -- a-addr )  $____ >in !

\ a-addr is the address of a cell containing the offset in char-
\ acters from the start of the input buffer to the start of the
\ parse area.


: key  ( -- char )  something ;

\ Receive one character char from the user input device. Key-
\ board events that do not correspond to characters in the char-
\ acter set are discarded until a valid character is received,
\ and those events are subsequently unavailable.


: accept  ( c-addr +n1 -- +n2 )  something ;

\ Receive a string of at most +n1 characters from the user input
\ device, write the string to the memory region with starting
\ address c-addr, and display graphic characters as they are re-
\ ceived. Input terminates when a line terminator character is
\ received. When input terminates, nothing is appended to the
\ string, and the display is maintained. An ambiguous condition
\ exists if +n1 is zero or is greater than +32767.


: char  ( '<spaces>name' -- char )  something ;

\ Skip leading spaces and parse name delimited by a space. Put
\ the first character of name on the stack.


: word  ( '<chars>ccc<char>' char -- c-addr )  something ;

\ Skip leading delimiters and parse ccc delimited by char. Write
\ the parsed word to a dedicated buffer as a counted string, and
\ put the address of the counted string on the stack. If the
\ parse area was empty or contained no characters other than the
\ delimiter, the resulting string has zero length. A program may
\ replace characters in the string. An ambiguous condition ex-
\ ists if the length of the parsed string is greater than 65535.



\ Core-Ext Text Input


: \  ( 'ccc<eol>' -- ) ( Run: -- )  something ;

\ Parse the remainder of the parse area. This causes the outer
\ interpreter to skip past the text that begins with \ and ends
\ at the end of the line.


: parse  ( 'ccc<char>' char -- c-addr u )  something ;

\ Parse ccc delimited by the delimiter char. c-addr is the ad-
\ dress of the parsed string within the input buffer, and u is
\ the string length. If the parse area was empty, the resulting
\ string has zero length.


: parse-name  ( '<spaces>name<space>' -- c-addr u )  something ;

\ Skip leading spaces and parse name delimited by a space.
\ c-addr is the address of the parsed string within the input
\ buffer, and u is the string length. If the parse area was emp-
\ ty or contains only white space, the resulting string has zero
\ length.


value source-id  ( -- 0 | -1 )  0 to source-id

\ If the input source is the user input device, put 0 on the
\ stack. If the input source is a string via EVALUATE, put -1
\ on the stack.


: save-input  ( -- xn...x1 n )  something ;

\ x1 through xn describe the current state of the input source
\ specification for later use by RESTORE-INPUT.


: restore-input  ( xn...x1 n -- flag )  something ;

\ Attempt to restore the input source specification to the state
\ described by x1 through xn. flag is true if the input source
\ specification cannot be so restored. An ambiguous condition
\ exists if the input source represented by the arguments is not
\ the same as the current input source.


: refill  ( -- flag )  something ;

\ Attempt to fill the input buffer from the input source.
\ When the input source is the user input device, attempt to re-
\ ceive input into the terminal input buffer. If successful,
\ make the result the input buffer, set >IN to zero, and return
\ a true flag. Receipt of a line containing no characters is
\ considered successful. If no input is available from the input
\ source, return false.
\ When the input source is a string via EVALUATE, return false
\ and perform no other action.



\ Core Query


: depth  ( -- +n )  something ;

\ +n is the number of single-cell values contained in the data
\ stack before +n was placed on the stack.


: environment?  ( c-addr u -- false | i*x true )  something ;

\ Description of something goes here



\ Core-Ext Query


: unused  ( -- u )  something ;

\ u is the number of address units remaining in the space ad-
\ dressed by HERE.



\ Core Execution Token


: execute  ( i*x xt -- j*x )  something ;

\ Remove xt from the stack and execute the word corresponding to
\ xt.


: '  ( '<spaces>name' -- xt )  something ;

\ Skip leading spaces and parse name delimited by a space. Find
\ name and put the corresponding execution token on the stack.
\ When interpreting, ' xyz EXECUTE is equivalent to xyz.


: find  ( c-addr -- c-addr 0 | xt 1 | xt -1 )  something ;

\ Attempt to match the counted string at c-addr to the name of a
\ dictionary definition. If the definition is not found, return
\ c-addr and zero. If the definition is found, return the corre-
\ sponding execution token. If the definition is immediate, also
\ return 1. Otherwise, also return -1. For a given string, the
\ values returned by FIND while compiling may differ from those
\ returned while not compiling.


: >body  ( xt -- a-addr )  something ;

\ a-addr is the address of the data field of the definition with
\ execution token xt. An ambiguous condition exists if xt is not
\ the execution token of a word defined by CREATE.



\ Core-Ext Execution Token


: defer@  ( xt1 -- xt2 )  something ;

\ xt2 is the execution token xt1 is set to execute. An ambiguous
\ condition exists if xt1 is not the execution token of a word
\ defined by DEFER, or if xt1 has not been set to execute an xt.


: defer!  ( xt1 xt2 -- )  something ;

\ Set the word xt1 to execute xt2. An ambiguous condition exists
\ if xt1 is not for a word defined by DEFER.


: is  ( Inter: '<spaces>name' xt -- )
  ( Compi: '<spaces>name' -- ) ( Run: xt -- )
  something ;

\ Interpretation: Description of something goes here

\ Compilation: Description of something goes here

\ Runtime: Description of something goes here


: action-of  ( Inter: '<spaces>name' -- xt )
  ( Compi: '<spaces>name' -- ) ( Run: -- xt )
  something ;

\ Interpretation: Description of something goes here

\ Compilation: Description of something goes here

\ Runtime: Description of something goes here



\ Core Compiler


: ,  ( x -- )  something ;

\ Reserve one cell of dictionary space and store x in the cell.
\ If the dictionary pointer is aligned when , begins execution,
\ it will remain aligned when , finishes execution. An ambiguous
\ condition exists if the dictionary pointer is not aligned pri-
\ or to the execution of ,.


synonym c, ,  ( char -- )

\ Reserve space for one character in the dictionary and store
\ char in the space. If the dictionary pointer is character a-
\ ligned when c, begins execution, it will remain character a-
\ ligned when c, finishes execution. Because Opforth characters
\ and cells are the same size, this operation is equivalent to
\ ,. An ambiguous condition exists if the dictionary pointer is
\ not character aligned prior to the execution of c,.


: allot  ( n -- )  something ;

\ If n is greater than zero, reserve n address units of dictio-
\ nary space. If n is less than zero, release |n| address units
\ of dictionary space. If n is zero, leave the dictionary point-
\ er unchanged. If the dictionary pointer is aligned and n is a
\ multiple of the size of a cell when ALLOT begins execution, it
\ will remain aligned when ALLOT finishes execution.


: align  ( -- )  ; immediate

\ If the dictionary pointer is not aligned, reserve enough space
\ to align it. Because the size of an Opforth cell is one ad-
\ dress unit, all addresses are aligned and this operation does
\ nothing.


: here  ( -- addr )  something ;

\ addr is the dictionary pointer.


: [  ( -- )  something ; immediate

\ Enter interpretation state.


: ]  ( -- )  something ;

\ Enter compilation state.


variable state  ( -- a-addr )  false state !

\ a-addr is the address of a cell containing the compilation-
\ state flag. STATE is true when in compilation state. STATE is
\ false otherwise. Only the following standard words alter the
\ value of STATE: : (colon), ; (semicolon), ABORT, QUIT,
\ :NONAME, [ (left-bracket), ] (right-bracket).


: postpone  ( Compi: '<spaces>name' -- )  something ;

\ Skip leading spaces and parse name delimited by a space. Find
\ name in the dictionary. Compile the compilation semantics of
\ name. An ambiguous condition exists if name is not found.


: literal  ( Compi: x -- ) ( Run: -- x )  something ;

\ Interpretation: Undefined

\ Compilation: Compile the following runtime semantics.

\ Runtime: Put x on the stack.


: [char]  ( Compi: '<spaces>name' -- ) ( Run: -- char )
  something ;

\ Interpretation: Undefined

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Compile the following runtime semantics.

\ Runtime: Put the value of the first character of name on the
\ stack.


: [']  ( Compi: '<spaces>name' -- ) ( Run: -- xt )  something ;

\ Interpretation: Undefined

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Find name in the dictionary. Compile the following run-
\ time semantics. An ambiguous condition exists if name is not
\ found.

\ Runtime: Put the execution token corresponding to name onto
\ the stack. The execution token returned by the compiled phrase
\ "['] X" is the same token returned by "' X" outside of compi-
\ lation state.


: s"  ( Compi: 'ccc<quote>' -- ) ( Run: -- c-addr u )
  something ;

\ Interpretation: Undefined

\ Compilation: Parse ccc delimited by " (double-quote). Compile
\ the following runtime semantics.

\ Runtime: Return the address c-addr and length u of a string
\ consisting of the characters ccc. A program shall not alter
\ the returned string.



\ Core-Ext Compiler


: s\"  ( Compi: 'ccc<quote>' -- )  something ;

\ Interpretation: Undefined

\ Compilation: Parse ccc delimited by " (double-quote) using the
\ translation rules below. Compile the following runtime seman-
\ tics.

\ Runtime: Return the address c-addr and length u of a string
\ consisting of the characters ccc. A program shall not alter
\ the returned string.
\ Translation Rules: something


: c"  ( Compi: 'ccc<quote>' -- ) ( Run: -- c-addr )  something ;

\ Interpretation: Undefined

\ Compilation: Parse ccc delimited by " (double-quote) and com-
\ pile the following runtime semantics.

\ Runtime: Return the address of a counted string consisting of
\ the characters ccc. A program shall not alter the returned
\ string.


: compile,  ( Compi: -- ) ( Exe: xt -- )  something ;

\ Interpretation: Undefined

\ Compilation: Compile the following execution semantics.

\ Execution: Compile the execution semantics of the definition
\ represented by xt.


: [compile]  ( Compi: '<spaces>name' -- )  something ;

\ Interpretation: Undefined

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Find name in the dictionary. If name has other than de-
\ fault compilation semantics, compile the compilation seman-
\ tics. Otherwise, compile the execution semantics of name. An
\ ambiguous condition exists if name is not found.

\ Note: This word is obsolescent and is included for compatibil-
\ ity with existing Forth code.



\ Core Definition


: :  ( '<spaces>name' -- colon-sys )
  ( Initi: i*x R: -- i*x R:nest-sys ) ( Exe: i*x -- j*x )
  something ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a new definition for name. Enter compilation state, start
\ the current definition, and produce colon-sys. Compile the ex-
\ ecution semantics described below. The execution semantics
\ will be determined by the words compiled into the body of the
\ definition. The current definition shall not be findable in
\ the dictionary until it is ended.
\ (Decision about something: Is the definition made findable by
\ the execution of DOES>?)

\ Initiation: Put the return address nest-sys on the return
\ stack. The stack effects i*x represent arguments to name.

\ name Execution: Execute the definition. The stack effects i*x
\ and j*x represent arguments to and results from name, respect-
\ ively.


: ;  ( Compi: colon-sys -- ) ( Run: R:nest-sys -- R: )
  something ;

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below.
\ End the current definition, allow it to be found in the dic-
\ tionary, enter interpretation state, and consume colon-sys.
\ If the dictionary pointer is not aligned, reserve enough space
\ to align it.

\ Runtime: Return to the calling definition specified by the re-
\ turn address nest-sys.


: immediate  ( -- )  something ;

\ Make the most recent definition an immediate word. An ambigu-
\ ous condition exists if the most recent definition does not
\ have a name or if it was defined by SYNONYM.


: constant  ( '<spaces>name' x -- ) ( Exe: -- x )  something ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the following execution seman-
\ tics.

\ name Execution: Put x on the stack.


: variable  ( '<spaces>name' -- ) ( Exe: -- a-addr ) something ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the following execution seman-
\ tics.

\ name Execution: a-addr is the address of the reserved cell. A
\ program is responsible for initializing the contents of the
\ reserved cell.


: create  ( '<spaces>name' -- ) ( Exe: -- a-addr )  something ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below. If the dictionary pointer is not aligned, re-
\ serve enough space to align it. The new dictionary pointer de-
\ fines the data field of name. CREATE does not allocate dictio-
\ nary space in the data field.

\ Execution: a-addr is the address of the data field of name.
\ The execution semantics of name may be extended by using
\ DOES>.


: does>  ( Compi: colon-sys1 -- colon-sys2 )
  ( Run: R:nest-sys1 -- R: )
  ( Initi: i*x R: -- i*x a-addr R:nest-sys2 )
  ( Exe: i*x -- j*x )
  something ;

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below.
\ ( comment about something related to whether the definition
\ becomes findable in the dictionary )
\ Consume colon-sys1 and produce colon-sys2. Compile the initia-
\ tion semantics described below.

\ Runtime: Replace the execution semantics of the most recent
\ definition, referred to as name, with the name execution se-
\ mantics described below. Return control to the calling defini-
\ tion specified by nest-sys1. An ambiguous condition exists if
\ name was not defined by CREATE or a user-defined word that
\ calls CREATE.

\ Initiation: Put the return address nest-sys2 on the return
\ stack. Put the address of the data field of name on the data
\ stack. The stack effects i*x represent arguments to name.

\ name Execution: Execute the portion of the definition that be-
\ gins with the initiation semantics appended by the DOES> that
\ modified name. The stack effects i*x and j*x represent argu-
\ ments to and results from name, respectively.



\ Core-Ext Definition

: :noname  ( -- xt colon-sys )
  ( Initi: i*x R: -- i*x R:nest-sys ) ( Exe: i*x -- j*x )
  something ;

\ Create an execution token xt, enter compilation state, start
\ the current definition, and produce colon-sys. Compile the
\ initiation semantics described below. The execution semantics
\ of xt will be determined by the words compiled into the body
\ of the definition. This definition can be executed later by
\ using xt EXECUTE.

\ Initiation: Put the return address nest-sys on the return
\ stack. The stack effects i*x represent arguments to xt.

\ xt Execution: Execute the definition specified by xt. The
\ stack effects i*x and j*x represent arguments to and results
\ from xt, respectively.


: buffer:  ( '<spaces>name' u -- ) ( Exe: -- a-addr )
  something ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below. Reserve u address units at an aligned address.
\ ( something about contiguity of the region for Opforth and
\ other Forth systems )

\ name Execution: a-addr is the address of the space reserved by
\ BUFFER: when it defined name. A program is responsible for
\ initializing the contents.


: value  ( '<spaces>name' x -- ) ( Exe: -- x ) ( to: x -- )
  something ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below. Write x to the data field.

\ name Execution: Put x on the stack. The contents of x are
\ those assigned by the most recent execution of the phrase
\ x TO name. If x TO name has not been executed, the contents
\ of x are those assigned when name was created.

\ TO name Runtime: Write x to the data field of name.


: to  ( Inter: '<spaces>name' i*x -- )
  ( Compi: '<spaces>name' -- )
  something ;

\ Interpretation: Skip leading spaces and parse name delimited
\ by a space. Perform the "TO name Runtime" semantics given in
\ the definition of the defining word of name. An ambiguous con-
\ dition exists if name was not defined by a word with "TO name
\ Runtime" semantics.

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Compile the "TO name Runtime" semantics given in the
\ definition of the defining word of name. An ambiguous condi-
\ tion exists if name was not defined by a word with "TO name
\ Runtime" semantics.

\ Note: An ambiguous condition exists if any of POSTPONE,
\ [COMPILE], or ', or ['] are applied to TO.


: defer  ( '<spaces>name' -- ) ( Exe: i*x -- j*x )
  something ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below.

\ name Execution: Execute the xt that name is set to execute. An
\ ambuguous condition exists if name has not been set to execute
\ an xt.


: marker  ( '<spaces>name' -- ) ( Exe: -- )
  something ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics defined
\ below.

\ name Execution: Restore all dictionary allocation and search
\ order pointers to the state they had just prior to the defini-
\ tion of name. Remove the definition and all subsequent defini-
\ tions. Restoration of any structures still existing that could
\ refer to deleted definitions or deallocated data space is not
\ necessarily provided. No other contextual information (such as
\ the base of the number system) is affected.
