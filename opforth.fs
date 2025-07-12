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
\ 2dup     Int: x1 x2 -- x1 x2 x1 x2
\          Com: --  Run: x1 x2 -- x1 x2 x1 x2
\ 2swap    x1 x2 x3 x4 -- x3 x4 x1 x2
\ 2over    x1 x2 x3 x4 -- x1 x2 x3 x4 x1 x2
\ >r       Com: --  Exe: x R: -- R:x
\ r>       Com: --  Exe: R:x -- x R:
\ r@       Com: --  Exe: R:x -- x R:x


\ Core-Ext Stack

\ nip     x1 x2 -- x2
\ tuck    x1 x2 -- x2 x1 x2
\ pick    xu...x1 x0 u -- xu...x1 x0 xu
\ roll    xu xu-1...x0 u -- xu-1...x0 xu
\ 2>r     Com: --  Exe: x1 x2 R: -- R:x1 R:x2
\ 2r>     Com: --  Exe: R:x1 R:x2 -- x1 x2 R:
\ 2r@     Com: --  Exe: R:x1 R:x2 -- x1 x2 R:x1 R:x2


\ Helper Stack

\ -rot      x1 x2 x3 -- x3 x1 x2
\ third     x1 x2 x3 -- x1 x2 x3 x1
\ swap>r    x1 x2 R: -- x2 R:x1
\ r>swap    x1 R:x2 -- x2 x1 R:
\ rdrop     R:x -- R:
\ sp0       -- +n
\ rp0       -- +n
\ sp@       -- +n
\ rp@       -- +n
\ sp!       i*x +n -- j*x
\ rp!       +n R:i*x -- R:j*x


\ Core Arithmetic

\ +         n1|u1 n2|u2 -- n3|u3
\ -         n1|u1 n2|u2 -- n3|u3
\ 1+        n1|u1 -- n2|u2
\ 1-        n1|u1 -- n2|u2
\ negate    n1 -- n2
\ abs       n -- u
\ s>d       n -- d
\ *         n1|u1 n2|u2 -- n3|u3
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


\ Helper Arithmetic

\ non-restoring    ud u1 -- n u2
\ ud/mod           ud1 u1 -- ud2 u2
\ +c               n1|u2 n2|u2 -- n3|u3 0|1
\ -c               n1|u2 n2|u2 -- n3|u3 0|-1


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
\ within    n1 n2 n3 | u1 u2 u3 -- flag


\ Helper Number Test

\ 0<=    n -- flag
\ 0>=    n -- flag
\ odd    n|u -- flag


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


\ Helper Bitwise Logic

\ u2/     x1 -- x2
\ 2*<<    x1 x2 -- x1 x3
\ >>2/    x1 x2 -- x1 x3


\ Core Address Math

\ cells      n1 -- n2
\ chars      n1 -- n2
\ cell+      a-addr1 -- a-addr2
\ char+      c-addr1 -- c-addr2
\ aligned    addr -- a-addr
\ count      c-addr1 -- c-addr2 u


\ Helper Address Math

\ cell     -- n
\ cell-    a-addr1 -- a-addr2


\ Core Memory

\ @       a-addr -- x
\ !       Int: x a-addr --
\         Com: --  Run: x a-addr --
\ c@      c-addr -- char
\ c!      char c-addr --
\ 2@      a-addr -- x1 x2
\ 2!      x1 x2 a-addr --
\ +!      n|u a-addr --
\ move    addr1 addr2 u --
\ fill    c-addr u char --


\ Core-Ext Memory

\ erase    addr u --
\ pad      -- c-addr


\ Helper Memory

\ @+       a-addr1 -- a-addr2 x
\ !+       x a-addr1 -- a-addr2
\ @-       a-addr1 -- a-addr2 x
\ !-       x a-addr1 -- a-addr2
\ c@+      c-addr1 -- c-addr2 char
\ c!+      char c-addr1 -- c-addr2
\ c@-      c-addr1 -- c-addr2 char
\ c!-      char c-addr1 -- c-addr2


\ Core Text Display

\ ."        Int: 'ccc<quote>' --
\           Com: 'ccc<quote>' --  Run: --
\ emit      x --
\ type      c-addr u --
\ cr        --
\ bl        -- char
\ space     --
\ spaces    n --


\ Core-Ext Text Display

\ .(    'ccc<right-paren>' --


\ Helper Text Display

\ textdisplay    -- c-addr
\ text-x         -- a-addr
\ text-y         -- a-addr
\ text-xmax      -- u
\ text-ymax      -- u


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


\ Helper Numeric String

\ holdptr    -- c-addr
\ holdbuf    -- c-addr


\ Core Text Input

\ (         Int: 'ccc<right-paren>' --
\           Com: 'ccc<right-paren>' --  Run: --
\ source    -- c-addr u
\ >in       -- a-addr
\ key       -- char
\ accept    c-addr +n1 -- +n2
\ char      '<spaces>name' -- char
\ word      '<chars>ccc<char>' char -- c-addr


\ Core-Ext Text Input

\ \                Com,Exe: 'ccc<eol>' --
\ parse            'ccc<char>' char -- c-addr u
\ parse-name       '<spaces>name<space>' -- c-addr u
\ source-id        -- 0 | -1 | fileid
\ save-input       -- xn...x1 n
\ restore-input    xn...x1 n -- flag
\ refill           -- flag


\ Helper Text Input

\ textinbuf       -- c-addr
\ #textinbuf      -- u
\ textindev       -- c-addr
\ keydev          -- a-addr
\ invalidchar?    char -- flag
\ space?          char -- flag
\ eol?            char -- flag
\ hex?            char -- flag
\ xt-skip         addr1 n1 xt -- addr2 n2
\ parse-area      -- c-addr u
\ advance>in      c-addr u -- c-addr u


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
\ is           Int: '<spaces>name' xt --
\              Com: '<spaces>name' --  Run: xt --
\ action-of    Int: '<spaces>name' -- xt
\              Com: '<spaces>name' --  Run: -- xt


\ Helper Execution Token

\ interpretation    Com: -- a-addr
\ compilation       Com: a-addr --


\ Core Compiler

\ here        -- addr
\ ,           x --
\ c,          char --
\ allot       n --
\ align       --
\ [           --
\ ]           --
\ state       -- a-addr
\ postpone    Com: '<spaces>name' --
\ literal     Com: x --  Run: -- x
\ [char]      Com: '<spaces>name' --  Run: -- char
\ [']         Com: '<spaces>name' --  Run: -- xt
\ s"          Int: 'ccc<quote>' -- c-addr u
\             Com: 'ccc<quote>' --  Run: -- c-addr u


\ Core-Ext Compiler

\ s\"          Com: 'ccc<quote>' --
\ c"           Com: 'ccc<quote>' --  Run: -- c-addr
\ compile,     Com: --  Exe: xt --
\ [compile]    Com: '<spaces>name' --


\ Helper Compiler

\ lit         -- x
\ dp          -- a-addr
\ dpmax       -- a-addr
\ 2,          x1 x2 --
\ s"buf       -- c-addr
\ s\"buf      -- c-addr
\ s\"ptr      -- c-addr
\ s\"append   char --
\ \x          c-addr1 u1 -- c-addr2 u2 | c-addr2 u2 char


\ Core Definition

\ :            '<spaces>name' -- flag
\              Ini: i*x R: -- i*x R:a-addr  Exe: i*x -- j*x
\ ;            Com: flag --  Run: R:a-addr -- R:
\ immediate    --
\ constant     '<spaces>name' x --  Exe: -- x
\ variable     '<spaces>name' --  Exe: -- a-addr
\ create       '<spaces>name' --  Exe: -- a-addr
\ does>        Com: flag1 -- flag2
\              Run: R:nest-sys1 -- R:
\              Ini: i*x R: -- i*x a-addr R:nest-sys2
\              Exe: i*x -- j*x


\ Core-Ext Definition

\ :noname    -- xt flag
\            Ini: i*x R: -- i*x R:a-addr  Exe: i*x -- j*x
\ buffer:    '<spaces>name' u --  Exe: -- a-addr
\ value      '<spaces>name' x --  Exe: -- x  To: x --
\ to         Int: '<spaces>name' i*x --
\            Com: '<spaces>name' --
\ defer      '<spaces>name' --  Exe: i*x -- j*x
\ marker     '<spaces>name' --  Exe: --


\ Helper Definition

\ define               '<spaces>name<space>' x --
\ opcode               '<spaces>name<space>' x --
\ compile-only         --
\ findable             --
\ dlink                -- a-addr
\ deflink              -- a-addr
\ defaddr              -- a-addr
\ length-mask          -- x
\ flags-mask           -- x
\ immediate-flag       -- x
\ compile-only-flag    -- x
\ combined-flag        -- x
\ create-flag          -- x
\ defer-flag           -- x
\ value-flag           -- x
\ 2value-flag          -- x


\ Core Control Flow

\ if         Com: -- orig  Run: x --
\ then       Com: orig --  Run: --
\ else       Com: orig1 -- orig2  Run: --
\ begin      Com: -- dest  Run: --
\ until      Com: dest --  Run: x --
\ while      Com: dest -- orig dest  Run: x --
\ repeat     Com: orig dest --  Run: --
\ do         Com: -- false a-addr
\            Run: n1|u1 n2|u2 R: -- R:n1|u1 R:n2|u2
\ loop       Com: flag a-addr --
\            Run: R:n1|u1 R:n2|u2 -- R:|n1|u1 R:|n3|u3
\ +loop      Com: flag a-addr --
\            Run: n R:n1|u1 R:n2|u2 -- R:|n1|u1 R:|n3|u3
\ i          Com: --
\            Exe: R:n|u -- n|u R:n|u
\ j          Com: --
\            Exe: R:n1|u1 R:n2|u2 R:n3|u3 --
\            n3|u3 R:n1|u1 R:n2|u2 R:n3|u3
\ leave      Com: --  Exe: R:n1|u1 R:n2|u2 -- R:
\ unloop     Com: --  Exe: R:n1|u1 R:n2|u2 -- R:
\ exit       Com: --  Exe: R:a-addr -- R:
\ recurse    Com: --


\ Core-Ext Control Flow

\ again      Com: dest --  Run: --
\ ?do        Com: -- true a-addr
\            Run: n1|u1 n2|u2 R: -- R:|n1|u1 R:|n2|u2
\ case       Com: -- case-sys  Run: --
\ of         Com: -- of-sys  Run: x1 x2 -- |x1
\ endof      Com: case-sys1 of-sys -- case-sys2  Run: --
\ endcase    Com: case-sys --  Run: x --


\ Helper Control Flow

\ nop              Com: --  Exe: --
\ branch           Com: --  Exe: --
\ ?branch          Com: --  Exe: x --
\ ?loop            Com: --  Exe: R:n|u -- |loop-sys
\ leave-link       -- a-addr
\ resolve-leave    Com: --  Exe: --


\ Core Outer Interpreter

\ quit        R:i*x -- R:
\ abort       i*x R:j*x -- R:
\ abort"      Com: 'ccc<quote>' --
\             Run: i*x x1 R:j*x -- |i*x R:|j*x
\ evaluate    i*x c-addr u -- j*x


\ Helper Outer Interpreter

\ interpret    '<chars>ccc<char>' i*x -- j*x


\ Exception

\ catch    i*x xt -- j*x 0 | i*x n
\ throw    k*x n -- k*x | i*x n


\ Helper Exception

\ handler      -- a-addr
\ errorbuf     -- c-addr
\ #errorbuf    -- u


\ Double

\ m+           d1|ud1 n -- d2|ud2
\ d+           d1|ud1 d2|ud2 -- d3|ud3
\ d-           d1|ud1 d2|ud2 -- d3|ud3
\ dnegate      d1 -- d2
\ dabs         d -- ud
\ d>s          d -- n
\ m*/          d1 n1 +n2 -- d2
\ d0=          Int: xd -- flag
\              Com: --  Run: xd -- flag
\ d0<          Int: d -- flag
\              Com: --  Run: d -- flag
\ d=           xd1 xd2 -- flag
\ d<           d1 d2 -- flag
\ dmax         d1 d2 -- d3
\ dmin         d1 d2 -- d3
\ d2*          xd1 -- xd2
\ d2/          xd1 -- xd2
\ d.           d --
\ d.r          d n --
\ 2literal     Com: x1 x2 --  Run: -- x1 x2
\ 2constant    '<spaces>name' x1 x2 --  Exe: -- x1 x2
\ 2variable    '<spaces>name' --  Exe: -- a-addr


\ Double-Ext

\ 2rot      x1 x2 x3 x4 x5 x6 -- x3 x4 x5 x6 x1 x2
\ du<       ud1 ud2 -- flag
\ 2value    '<spaces>name' x1 x2 --  Exe: -- x1 x2  To: x1 x2 --


\ Helper Double

\ m-      d1|ud1 n -- d2|ud2
\ ud2/    xd1 -- xd2
\ ud.r    ud n --


\ String

\ cmove         c-addr1 c-addr2 u --
\ cmove>        c-addr1 c-addr2 u --
\ blank         c-addr u --
\ sliteral      Com: c-addr1 u --  Run: -- c-addr2 u
\ /string       c-addr1 u1 n2 -- c-addr2 u2
\ -trailing     c-addr u1 -- c-addr u2
\ compare       c-addr1 u1 c-addr2 u2 -- n
\ search        c-addr1 u1 c-addr2 u2 -- c-addr3 u3 flag


\ String-Ext

\ replaces      c-addr1 u1 c-addr2 u2 --
\ unescape      c-addr1 u1 c-addr2 -- c-addr2 u2
\ substitute    c-addr1 u1 c-addr2 u2 -- c-addr3 u3 n


\ Helper String

\ /char      ( c-addr1 u1 -- c-addr2 u2 char )
\ string,    ( c-addr u -- )


\ Facility

\ page     --
\ at-xy    u1 u2 --
\ key?     -- flag


\ Facility-Ext

\ emit?              -- flag
\ ekey               -- x
\ ekey?              -- flag
\ ekey>char          x -- x false | char true
\ ekey>fkey          x -- u flag
\ k-up               -- u
\ k-down             -- u
\ k-right            -- u
\ k-left             -- u
\ k-home             -- u
\ k-end              -- u
\ k-delete           -- u
\ k-insert           -- u
\ k-ctrl-mask        -- u
\ k-alt-mask         -- u
\ k-shift-mask       -- u
\ k-f1               -- u
\ k-f2               -- u
\ k-f3               -- u
\ k-f4               -- u
\ k-f5               -- u
\ k-f6               -- u
\ k-f7               -- u
\ k-f8               -- u
\ k-f9               -- u
\ k-f10              -- u
\ k-f11              -- u
\ k-f12              -- u
\ begin-structure    '<spaces>name' -- struct-sys 0
\                    Exe: -- +n
\ end-structure      struct-sys +n --
\ cfield:            '<spaces>name' n1 -- n2
\                    Exe: addr1 -- addr2
\ field:             '<spaces>name' n1 -- n2
\                    Exe: addr1 -- addr2
\ +field             '<spaces>name' n1 n2 -- n3
\                    Exe: addr1 -- addr2
\ ms                 u --


\ Block

\ blk             -- a-addr
\ block           u -- a-addr
\ buffer          u -- a-addr
\ flush           --
\ load            i*x u -- j*x
\ save-buffers    --
\ update          --


\ Block-Ext

\ empty-buffers    --
\ list             --
\ scr              -- a-addr
\ thru             i*x u1 u2 -- j*x


\ File

\ open-file          c-addr u fam -- fileid ior
\ close-file         fileid -- ior
\ create-file        c-addr u fam -- fileid ior
\ delete-file        c-adddr u -- ior
\ rename-file        c-addr1 u1 c-addr2 u2 -- ior
\ reposition-file    ud fileid -- ior
\ write-file         c-addr u fileid -- ior
\ write-line         c-addr u fileid -- ior
\ bin                fam1 -- fam2


\ Tools

\ .s      --
\ ?       a-addr --
\ dump    addr u --
\ see     '<spaces>name' --
\ words   --


\ Tools-Ext

\ synonym    '<spaces>newname' '<spaces>oldname' --


\ Core Stack Words


$____ opcode drop  ( x -- )

\ Remove the top stack item.


$____ opcode dup  ( x -- x x )

\ Duplicate the top stack item.


$____ opcode swap  ( x1 x2 -- x2 x1 )

\ Exchange the top two stack items.


$____ opcode over  ( x1 x2 -- x1 x2 x1 )

\ Put a copy of the second stack item on top of the stack.


: rot  ( x1 x2 x3 -- x2 x3 x1 )
  interpretation
    >r swap r>swap
  compilation
    postpone >r
    postpone swap
    postpone r>swap ; immediate

\ Rotate the top three stack items to bring the third item to
\ the top.


$____ opcode ?dup  ( x -- x x | 0 )

\ Duplicate the top stack item if it is nonzero.


$____ opcode 2drop  ( x1 x2 -- )

\ Remove the top two stack items.


: 2dup  ( Int: x1 x2 -- x1 x2 x1 x2 )
        ( Com: -- ) ( Run: x1 x2 -- x1 x2 x1 x2 )
  interpretation
    over over
  compilation
    postpone over
    postpone over ; immediate

\ Duplicate the cell pair on top of the stack.


: 2swap  ( x1 x2 x3 x4 -- x3 x4 x1 x2 )  rot >r rot r> ;

\ Exchange the top two cell pairs on the stack.


: 2over  ( x1 x2 x3 x4 -- x1 x2 x3 x4 x1 x2 )
  2>r 2dup 2r> 2swap ;

\ Put a copy of the second cell pair, x1 x2, on top of the
\ stack.


$____ opcode >r  ( Com: -- ) ( Exe: x R: -- R:x )
  compile-only

\ Interpretation: Undefined

\ Execution: Transfer the top data stack item to the return
\ stack.


$____ opcode r>  ( Com: -- ) ( Exe: R:x -- x R: )
  compile-only

\ Interpretation: Undefined

\ Execution: Transfer the top return stack item to the data
\ stack.


$____ opcode r@  ( Com: -- ) ( Exe: R:x -- x R:x )
  compile-only

\ Interpretation: Undefined

\ Execution: Put a copy of the top return stack item onto the
\ data stack.



\ Core-Ext Stack Words


$____ opcode nip  ( x1 x2 -- x2 )

\ Remove the second stack item.


$____ opcode tuck  ( x1 x2 -- x2 x1 x2 )

\ Insert a copy of the top stack item under the second stack
\ item.


: pick  ( xu...x1 x0 u -- xu...x1 x0 xu )  sp@ + 1+ @ ;

\ Remove u and put a copy of xu, the stack item indexed by u, on
\ top of the stack.

\ An ambiguous condition exists if there are fewer than u+2
\ items on the stack before PICK is executed.


: roll  ( xu xu-1...x0 u -- xu-1...x0 xu )
  dup if
  swap >r 1- recurse r> swap exit then
  drop ;

\ Remove u and rotate the top u+1 stack items to bring xu to the
\ top.


: 2>r  ( Com: -- ) ( Exe: x1 x2 R: -- R:x1 R:x2 )
  postpone swap>r
  postpone >r ; immediate compile-only

\ Interpretation: Undefined

\ Execution: Transfer the cell pair on top of the data stack to
\ the return stack.


: 2r>  ( Com: -- ) ( Exe: R:x1 R:x2 -- x1 x2 R: )
  postpone r>
  postpone r>swap ; immediate compile-only

\ Interpretation: Undefined

\ Execution: Transfer the cell pair on top of the return stack
\ to the data stack.


: 2r@  ( Com: -- ) ( Exe: R:x1 R:x2 -- x1 x2 R:x1 R:x2 )
  postpone r>
  postpone r>
  postpone 2dup
  postpone >r
  postpone >r
  postpone swap ; immediate compile-only

\ Interpretation: Undefined

\ Execution: Put a copy of the cell pair on top of the return
\ stack onto the data stack.



\ Helper Stack Words


: -rot  ( x1 x2 x3 -- x3 x1 x2 )
  interpretation
    swap>r swap r>
  compilation
    postpone swap>r
    postpone swap
    postpone r> ; immediate

\ Rotate the top three stack items to put the top item in the
\ third position.


: third  ( x1 x2 x3 -- x1 x2 x3 x1 )  >r over r> swap ;

\ Put a copy of the third stack item on top of the stack.


$____ opcode swap>r  ( x1 x2 R: -- x2 R:x1 )

\ Put the second data stack item on top of the return stack.


$____ opcode r>swap  ( x1 R:x2 -- x2 x1 R: )

\ Put the top return stack item under the top data stack item.


$____ opcode rdrop  ( R:x -- R: )

\ Remove the top return stack item.


$____ value sp0  ( -- +n )

\ +n is the number contained in the data stack pointer when the
\ data stack is empty.


$____ value rp0  ( -- +n )

\ +n is the number contained in the return stack pointer when
\ the return stack is empty.


$____ opcode sp@  ( -- +n )

\ +n is the number contained in the data stack pointer before +n
\ was placed on the data stack.


$____ opcode rp@  ( -- +n )

\ +n is the number contained in the return stack pointer.


$____ opcode sp!  ( i*x +n -- j*x )

\ Set the data stack pointer to +n.


$____ opcode rp!  ( +n R:i*x -- R:j*x )

\ Set the return stack pointer to +n.



\ Core Arithmetic Words


$____ opcode +  ( n1|u1 n2|u2 -- n3|u3 )

\ Add the top two integers on the stack. Any of the numbers may
\ be signed or unsigned.


$____ opcode -  ( n1|u1 n2|u2 -- n3|u3 )

\ Subtract the top integer on the stack from the second integer.
\ Any of the numbers may be signed or unsigned.


$____ opcode 1+  ( n1|u1 -- n2|u2 )

\ Add one to the number on top of the stack. The number may be
\ signed or unsigned.


$____ opcode 1-  ( nu1 -- nu2 )

\ Subtract one from the number on top of the stack. The number
\ may be signed or unsigned.


$____ opcode negate  ( n1 -- n2 )

\ n2 is the arithmetic inverse of n1 (i.e., n2 = 0 - n1).


$____ opcode abs  ( n -- u )

\ u is the absolute value of n.


$____ opcode s>d  ( n -- d )

\ d is the result of converting the single-cell signed integer n
\ to a double-cell signed integer with the same numeric value.


: *  ( n1|u1 n2|u2 -- n3|u3 )  m* drop ;

\ Multiply the top two integers on the stack. Any of the numbers
\ may be signed or unsigned.


: m*  ( n1 n2 -- d )
  >r 0
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r> and + d2/
  over odd if dnegate then ;

\ Multiply n1 by n2. d is the double-cell product. All numbers
\ and arithmetic are signed.


: um*  ( u1 u2 -- ud )
  >r 0
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r@ and + d2/
  over odd r> and + ;

\ Multiply u1 by u2. ud is the double-cell product. All numbers
\ and arithmetic are unsigned.


: /  ( n1 n2 -- n3 )  >r s>d r> sm/rem nip ;

\ Divide n1 by n2. n3 is the quotient.

\ An ambiguous condition exists if n2 is zero.


: mod  ( n1 n2 -- n3 )  >r s>d r> sm/rem drop ;

\ Divide n1 by n2. n3 is the remainder. If n1 and n2 differ in
\ sign, n3 is determined by symmetric division.

\ An ambiguous condition exists if n2 is zero.


: /mod  ( n1 n2 -- n3 n4 )  >r s>d r> sm/rem ;

\ Divide n1 by n2. n3 is the remainder and n4 is the quotient.
\ If n1 and n2 differ in sign, n3 and n4 are determined by sym-
\ metric division.

\ An ambiguous condition exists if n2 is zero.


: sm/rem  ( d n1 -- n2 n3 )
  dup 0< >r abs dup >r
  non-restoring
  over r@ + 0= if
  1- swap r@ + swap then
  rdrop r> if negate then ;

\ Divide d by n1. n2 is the remainder and n3 is the quotient. If
\ d and n1 differ in sign, n2 and n3 are determined by symmetric
\ division.

\ An ambiguous condition exists if n1 is zero or if n3 is out-
\ side the range of a single-cell signed integer.


: fm/mod  ( d n1 -- n2 n3 )
  2dup >r >r
  sm/rem
  over 0<> r> 0< r@ 0< xor and if
  1- swap r@ + swap then
  rdrop ;

\ Divide d by n1. n2 is the remainder and n3 is the quotient. If
\ d and n1 differ in sign, n2 and n3 are determined by floored
\ division.

\ An ambiguous condition exists if n1 is zero or if n3 is out-
\ side the range of a single-cell signed integer.


: um/mod  ( ud u1 -- u2 u3 )
  dup >r over >r
  non-restoring
  r> 0< if negate r@ rot - swap then
  over 0< if 1- swap r@ + swap then
  rdrop ;

\ Divide ud by u1. u2 is the remainder and u3 is the quotient.
\ All integers and arithmetic are unsigned.

\ An ambiguous condition exists if u1 is zero or if u3 is out-
\ side the range of a single-cell unsigned integer.


: */  ( n1 n2 n3 -- n4 )  >r m* r> sm/rem drop ;

\ Multiply n1 by n2 to produce an intermediate double-cell prod-
\ uct d, then divide d by n3. n4 is the quotient. If d and n3
\ differ in sign, n4 is determined by symmetric division.

\ An ambiguous condition exists if n3 is zero or if n4 is out-
\ side the range of a single-cell signed integer.


: */mod  ( n1 n2 n3 -- n4 n5 )  >r m* r> sm/rem ;

\ Multiply n1 by n2 to produce an intermediate double-cell prod-
\ uct d, then divide d by n3. n4 is the remainder and n3 is the
\ quotient. If d and n3 differ in sign, n4 and n5 are determined
\ by symmetric division.

\ An ambiguous condition exists if n3 is zero or if n5 is out-
\ side the range of a single-cell signed integer.



\ Helper Arithmetic Words


: non-restoring  ( ud u1 -- n u2 )
  -rot
  16 0 do
    dup 0<
    if
      d2* third +
    else
      d2* swap 1+ swap third -
    then
  loop
  swap 2* 1+ rot drop ;

\ Divide ud by u1. n is the remainder and u2 is the quotient.
\ After executing NON-RESTORING, a fixup step is necessary to
\ convert the negative remainder to positive and to adjust the
\ quotient accordingly.


: ud/mod  ( ud1 u1 -- u2 ud2 )
  >r 0 r@ um/mod
  r> swap >r um/mod
  r> ;

\ Divide ud1 by u1. u2 is the remainder and ud2 is the quotient.
\ All integers and arithmetic are unsigned.


$____ opcode +c  ( n1|u1 n2|u2 -- n3|u3 0|1 )

\ Add the top two stack items. Replace the second stack item
\ with the sum, and replace the top stack item with the carry.
\ The numbers may be signed or unsigned.


$____ opcode -c  ( n1|u1 n2|u2 -- n3|u3 0|-1 )

\ Subtract the top stack item from the second stack item. Re-
\ place the second stack item with the sum, and replace the top
\ stack item with the carry. The numbers may be signed or un-
\ signed.



\ Core Number Test Words


$____ opcode 0=  ( x -- flag )

\ If all bits of x are zero, flag is true. Otherwise flag is false.


$____ opcode 0<  ( n -- flag )

\ If n is less than zero, flag is true. Otherwise flag is false.


: =  ( x1 x2 -- flag )
  interpretation
    - 0=
  compilation
    postpone -
    postpone = ; immediate

\ If x1 is bit-for-bit the same as x2, flag is true. Otherwise
\ flag is false.


: <  ( n1 n2 -- flag )
  2dup 0<
  if
    0< if swap u< else false then
  else
    0< if u< else true then
  then ;

\ If n1 is less than n2, flag is true. Otherwise flag is false.


: >  ( n1 n2 -- flag )
  2dup 0<
  if
    0< if u< else true then
  else
    0< if swap u< else false then
  then ;

\ If n1 is greater than n2, flag is true. Otherwise flag is
\ false.


: u<  ( u1 u2 -- flag )
  interpretation
    - 0<
  compilation
    postpone -
    postpone 0< ; immediate

\ If u1 is less than u2, flag is true. Otherwise flag is false.


: max  ( n1 n2 -- n3 )  2dup < if nip then ;

\ Compare the top two integers on the stack. n3 is the integer
\ that is greater (closer to positive infinity).


: min  ( n1 n2 -- n3 )  2dup < if drop then ;

\ Compare the top two integers on the stack. n3 is the integer
\ that is lesser (closer to negative infinity).



\ Core-Ext Number Test Words


$____ opcode 0<>  ( x -- flag )

\ If all bits of x are zero, flag is false. Otherwise flag is
\ true.


$____ opcode 0>  ( n -- flag )

\ If n is greater than zero, flag is true. Otherwise flag is
\ false.


: <>  ( x1 x2 -- flag )
  interpretation
    - 0<>
  compilation
    postpone -
    postpone 0<> ; immediate

\ If x1 is bit-for-bit the same as x2, flag is false. Otherwise
\ flag is true.


: u>  ( u1 u2 -- flag )
  interpretation
    - 0>=
  compilation
    postpone -
    postpone 0>= ; immediate


\ If u1 is greater than u2, flag is true. Otherwise flag is
\ false.


: within  ( n1 n2 n3 | u1 u2 u3 -- flag )
  third u> >r swap u< r> and ;

\ flag is true if either of the following is true:

\ n2<n3 and n1 lies inside the interval [n2,n3)
\ n2>=n3 and n1 lies outside the interval [n3,n2)

\ Otherwise, flag is false. The integers can be signed or un-
\ signed, but not a mix of both.

\ An ambiguous condition exists if the integers are a mix of
\ signed and unsigned.



\ Helper Number Test Words


$____ opcode 0<=  ( n -- flag )

\ If n is less than or equal to zero, flag is true. Otherwise
\ flag is false.


$____ opcode 0>=  ( n -- flag )

\ If n is greater than or equal to zero, flag is true. Otherwise
\ flag is false.


$____ opcode odd  ( n|u -- flag )

\ If the least significant bit of the top stack item is 1, flag
\ is true. Otherwise flag is false.



\ Core Bitwise Logic Words


$____ opcode invert  ( x1 -- x2 )

\ x2 is the result of inverting all bits of x1.


$____ opcode and  ( x1 x2 -- x3 )

\ x3 is the bit-for-bit logical AND of x1 with x2.


$____ opcode or  ( x1 x2 -- x3 )

\ x3 is the bit-for-bit logical OR of x1 with x2.


$____ opcode xor  ( x1 x2 -- x3 )

\ x3 is the bit-for-bit logical XOR of x1 with x2.


$____ opcode 2*  ( x1 -- x2 )

\ x2 is the result of shifting all bits of x1 to the left by one
\ bit position. The vacated least significant bit becomes zero.


$____ opcode 2/  ( x1 -- x2 )

\ x2 is the result of shifting all bits of x1 to the right by
\ one bit position. The vacated most significant bit is un-
\ changed.


: lshift  ( x1 u -- x2 )  0 ?do 2* loop ;

\ x2 is the result of shifting all bits of x1 to the left by u
\ bit positions. The vacated least significant bits become ze-
\ roes.


: rshift  ( x1 u -- x2 )  0 ?do u2/ loop ;

\ x2 is the result of shifting all bits of x1 to the right by u
\ bit positions. The vacated most significant bits become ze-
\ roes.



\ Core-Ext Bitwise Logic Words


$____ opcode true  ( -- true )

\ Put a logical TRUE flag on the stack. All bits of the flag are
\ ones.


$____ opcode false  ( -- false )

\ Put a logical FALSE flag on the stack. All bits of the flag
\ are zeroes.



\ Helper Bitwise Logic Words


$____ opcode u2/  ( x1 -- x2 )

\ x2 is the result of shifting all bits of x1 to the right by
\ one bit position. The vacated most significant bit becomes ze-
\ ro.


$____ opcode 2*<<  ( x1 x2 -- x1 x3 )

\ x3 is the result of shifting all bits of x2 to the left by
\ one bit position. The vacated least significant bit of x3 is
\ filled with the value of the most significant bit of x1.


$____ opcode >>2/  ( x1 x2 -- x1 x3 )

\ x3 is the result of shifting all bits of x2 to the right by
\ one bit position. The vacated most significant bit of x3 is
\ filled with the value of the least significant bit of x1.



\ Core Address Math Words


synonym cells 2*  ( n1 -- n2 )

\ n2 is the size in address units of n1 cells.

\ In Opforth, the size of a cell is two address units, and there
\ is no distinction between address units and characters. CELLS
\ is used for compatibility with Forth systems that have differ-
\ ent cell sizes.


: chars  ( n1 -- n2 )  ; immediate

\ n2 is the size in address units of n1 characters.

\ In Opforth, there is no distinction between address units and
\ characters. CHARS is used for compatibility with Forth sys-
\ tems that have a character size of greater than one address
\ unit.


synonym cell+ 2+  ( a-addr1 -- a-addr2 )

\ Add the size of one cell in address units to the top stack
\ item.

\ In Opforth, the size of a cell is two address units, and there
\ is no distinction between address units and characters. CELL+
\ is used for compatibility with Forth systems that have differ-
\ ent cell sizes.


synonym char+ 1+  ( c-addr1 -- c-addr2 )

\ Add the size of one character in address units to the top
\ stack item.

\ In Opforth, there is no distinction between address units and
\ characters. CHAR+ is used for compatibility with Forth systems
\ that have a character size of greater than one address unit.


: aligned  ( addr -- a-addr )  dup odd if 1+ then ;

\ a-addr is the first aligned address greater than or equal to
\ addr.

\ In Opforth, all addresses are aligned addresses. ALIGNED is
\ used for compatibility with other Forth systems that may have
\ non-aligned addresses.


: count  ( c-addr1 -- c-addr2 u )  dup char+ swap c@ ;

\ Given a counted string located at c-addr1, return the non-
\ counted string representation. c-addr2 is the location of the
\ first character after the count character, and u is the string
\ length excluding the count character.



\ Helper Address Math Words


synonym cell 2  ( -- n )

\ n is the size of a cell in address units.

\ In Opforth, the size of a cell is two address units, and there
\ is no distinction between address units and characters.


synonym cell- 2-  ( a-addr1 -- a-addr2 )

\ Subtract the size of one cell in address units from the top
\ stack item.

\ In Opforth, the size of a cell is two address units, and there
\ is no distinction between address units and characters. CELL+
\ is used for compatibility with Forth systems that have differ-
\ ent cell sizes.



\ Core Memory Words


$____ opcode @  ( a-addr -- x )

\ Read the cell located at memory address a-addr and put the
\ cell on the stack in place of a-addr.


: !  ( Int: x a-addr -- )
     ( Com: -- ) ( Run: x a-addr -- )
  interpretation
    !+ drop
  compilation
    postpone !+
    postpone drop ; immediate

\ Write x to memory address a-addr. The top two stack items are
\ removed.


$____ opcode c@  ( c-addr -- char )

\ Read the character located at memory address c-addr and put
\ the character on the stack in place of c-addr.


$____ opcode c!  ( char c-addr -- )

\ Write char to memory address c-addr.


: 2@  ( a-addr -- x1 x2 )  dup cell+ @ swap @ ;

\ Read the cell pair located at memory address a-addr and put
\ the cell pair on the stack. x2 is the cell at a-addr and x1
\ is the next consecutive cell.


: 2!  ( x1 x2 a-addr -- )  tuck ! cell+ ! ;

\ Write the cell pair x1 x2 to memory address a-addr. x2 is
\ written to a-addr and x1 is written to the next consecutive
\ cell.


: +!  ( n|u a-addr -- )  tuck @ + swap ! ;

\ Add the second stack item to the single-cell integer located
\ at memory address a-addr, then store the sum to a-addr. Any of
\ the integers can be signed or unsigned.


: move  ( addr1 addr2 u -- )
  third third u< if cmove> else cmove then ;

\ If u is greater than zero, copy the contents of the u consecu-
\ tive address units of memory starting at addr1 to the u con-
\ secutive address units of memory starting at addr2. After MOVE
\ is executed, the u address units of memory starting at addr2
\ contain exactly what the u address units of memory starting at
\ addr1 contained before MOVE was executed.

\ In Opforth, there is no distinction between address units and
\ characters. This implementation of MOVE is incompatible with
\ Forth systems that have a character size of greater than one
\ address unit.


: fill  ( c-addr u char -- )
  -rot 0 ?do over swap c!+ loop 2drop ;

\ If u is greater than zero, write char to each of the u consec-
\ utive characters of memory beginning at address c-addr.



\ Core-Ext Memory Words


: erase  ( addr u -- )  0 ?do 0 swap c!+ loop drop ;

\ If u is greater than zero, clear all bits of the u consecutive
\ address units of memory starting at address addr.

\ In Opforth, there is no distinction between address units and
\ characters. This implementation of ERASE is incompatible with
\ Forth systems that have a character size of greater than one
\ address unit.


$____ constant pad  ( -- c-addr )

\ c-addr is the address of the pad, which is a region of memory
\ that can be used to hold data for intermediate processing.



\ Helper Memory Words


$____ opcode @+  ( a-addr1 -- a-addr2 x )

\ Read the cell located at a-addr1, add one to a-addr1, and put
\ the cell on top of the stack.


$____ opcode !+  ( x a-addr1 -- a-addr2 )

\ Write x to memory address a-addr1. Add one to a-addr1, and put
\ the result on top of the stack.


$____ opcode @-  ( a-addr1 -- a-addr2 x )

\ Read the cell located at a-addr1, add subtract one from
\ a-addr1, and put the cell on top of the stack.


$____ opcode !-  ( x a-addr1 -- a-addr2 )

\ Write x to memory address a-addr1. Subtract one from a-addr1,
\ and put the result on top of the stack.


$____ opcode c@+  ( c-addr1 -- c-addr2 char )

\ Read the character located at a-addr1, add one to a-addr1, and
\ put the character on top of the stack.


$____ opcode c!+  ( char c-addr1 -- c-addr2 )

\ Write char to memory address c-addr1. Add one to c-addr1 and
\ put the result on top of the stack.


$____ opcode c@-  ( c-addr1 -- c-addr2 char )

\ Read the character located at c-addr1, subtract one from
\ c-addr1, and put the character on top of the stack.


$___ opcode c!-  ( char c-addr1 -- c-addr2 )

\ Write char to memory addres c-addr1. Subtract one from
\ c-addr1 and put the result on top of the stack.



\ Core Text Display Words


: ."  ( Int: 'ccc<quote>' -- )
      ( Com: 'ccc<quote>' -- ) ( Run: -- )
  interpretation
    '"' parse type
  compilation
    '"' parse postpone sliteral
    postpone type ; immediate

\ Interpretation: Parse ccc delimited by " (double-quote). Dis-
\ play ccc.

\ Compilation: Parse ccc delimited by " (double-quote). Compile
\ the following runtime semantics.

\ Runtime: Display ccc.


: emit  ( x -- )
  dup validchar?
  if
    textdisplay c!
    text-x dup @ dup text-xmax =
    if drop 0 cr else 1+ then
    swap !
  then ;

\ If x is a graphic character, display x.


: type  ( c-addr u -- )
  dup 0> and 0
  ?do dup c@ emit char+ loop
  drop ;

\ If u is greater than zero, display the string that has ad-
\ ress c-addr and length u.


: cr  ( -- )
  text-y dup @ dup text-ymax =
  swap 1+ and swap ! ;

\ Position the text cursor at the beginning of the next line.


$0020 constant bl  ( -- char )

\ char is the code for the space character.

\ Because Opforth uses ASCII/UTF-8 and the size of an Opforth
\ character is 16 bits, char is $0020.


: space  ( -- )  bl emit ;

\ Display one space.


: spaces  ( n -- )  0 ?do space loop ;

\ If n is greater than zero, display n spaces.



\ Core-Ext Text Display Words


: .(  ( 'ccc<right-paren>' -- )  ')' parse type ; immediate

\ Interpretation: Perform the execution semantics described be-
\ low.

\ Compilation: Perform the execution semantics described below.

\ Execution: Parse ccc delimited by ) (right parenthesis). Dis-
\ play ccc.



\ Helper Text Display Words


$____ constant textdisplay  ( -- c-addr )

\ c-addr is the address of the text display device. Writing a
\ valid graphic character to c-addr (e.g. using EMIT) causes
\ the character to be displayed.


$____ constant text-x  ( -- a-addr )

\ a-addr is the hardware address of the text cursor x coordi-
\ nate, which is the column where the next character will be
\ displayed. A program can get the x coordinate by reading the
\ contents of a-addr, and a program can set the x coordinate
\ by writing a number to a-addr. 

\ An ambiguous condition exists if a program writes a number
\ to a-addr that is less than zero or greater than TEXT-XMAX.


$____ constant text-y  ( -- a-addr )

\ a-addr is the hardware address of the text cursor y coordi-
\ nate, which is the row where the next character will be dis-
\ played. A program can get the y coordinate by reading the
\ contents of a-addr, and a program can set the y coordinate
\ by writing a number to a-addr. 

\ An ambiguous condition exists if a program writes a number
\ to a-addr that is less than zero or greater than TEXT-YMAX.


#64 value text-xmax  ( -- u )

\ u is the number of columns available for displaying text char-
\ acters.


#20 value text-ymax  ( -- u )

\ u is the number of rows available for displaying text charac-
\ ters.



\ Core Numeric String Words


: .  ( n -- )  s>d d. ;

\ Display a text representation of the integer n followed by a
\ space.


: u.  ( u -- )  0 ud. ;

\ Display a text representation of the unsigned integer u fol-
\ lowed by a space.


: <#  ( -- )  holdbuf to holdptr ;

\ Start a number-to-string conversion. The closing delimiter for
\ the conversion expression is #>.


: #>  ( xd -- c-addr u )  2drop holdbuf dup holdptr - ;

\ Finish a number-to-string conversion by dropping xd and making
\ the string available. c-addr is the starting address of the
\ string, and u is the string length. A program may replace
\ characters within the string.


: #  ( ud1 -- ud2 )
  base @ ud/mod rot
  dup 9 u> [ char A char 9 1+ - ] literal and +
  '0' + hold ;

\ As part of a <# #> delimited number-to-string conversion, con-
\ vert one digit of ud1 using the following method. Divide ud1
\ by the number in BASE. Prepend a text representation of the
\ remainder to the string being built. ud2 is the quotient,
\ which can be used by a subsequent number-to-string conversion
\ word.

\ An ambiguous condition exists if # is executed outside of a
\ <# #> delimited conversion.


: #s  ( ud1 -- ud2 )  begin # 2dup or 0= until ;

\ As part of a <# #> delimited number-to-string conversion, con-
\ vert all digits of ud1 and prepend the digits to the string
\ being built. ud2 is zero.

\ An ambiguous condition exists if #S is executed outside of a
\ <# #> delimited conversion.


: hold  ( char -- )  holdptr tuck c! char- to holdptr ;

\ As part of a <# #> delimited number-to-string conversion, pre-
\ pend char to the string being built.

\ An ambiguous condition exists if HOLD is executed outside of a
\ <# #> delimited conversion.


: sign  ( n -- )  0< if '-' hold then ;

\ As part of a <# #> delimited number-to-string conversion, put
\ a minus sign at the beginning of the string being built if n
\ is negative.

\ An ambiguous condition exists if SIGN is executed outside of a
\ <# #> delimited conversion.


: decimal  ( -- )  #10 base ! ;

\ Set the base (radix) of the number system to ten.


variable base  ( -- a-addr )  decimal

\ a-addr is the address of a cell containing the base (radix) of
\ the number system. BASE is initialized to ten (decimal).


: >number  ( ud1 c-addr1 u1 -- ud2 c-addr2 u2 )
  2swap 2>r
  begin
    dup 0<> while
    drop /char'0' -
    'A' 'z' 1+ within [ char 'A' $a - ] literal and -
    'a' 'z' 1+ within [ char 'a' $a - ] literal and -
    dup base @ u< dup 0= if nip then while
    2r> swap m+ 2>r
  repeat then
  drop 2r> ;

\ Attempt to convert the string with address c-addr1 and length
\ u1 to a double-cell unsigned integer using ud1 as an accumula-
\ tor. For each character in the string, from left to right,
\ convert the character to a number using the radix in BASE, add
\ the number to ud1, and then multiply the result by the radix.

\ Conversion continues until the string is entirely converted or
\ a non-convertible character is encountered. + and - are non-
\ convertible characters.

\ ud2 is the converted number. c-addr2 is the location of the
\ first unconverted character or the first character past the
\ end of the string if the string was entirely converted. u2 is
\ the number of unconverted characters in the string.

\ An ambiguous condition exists if ud2 overflows during the con-
\ version.



\ Core-Ext Numeric String Words


: .r  ( n1 n2 -- )  >r s>d r> d.r ;

\ Display a text representation of n1 right-aligned in a field
\ n2 characters wide. If the number of characters required to
\ display n1 is greater than n2, all digits are displayed with
\ no leading spaces in a field as wide as necessary.


: u.r  ( u n -- )  0 swap ud.r ;

\ Display a text representation of u right-aligned in a field n
\ characters wide. If the number of characters required to dis-
\ play u is greater than n, all digits are displayed with no
\ leading spaces in a field as wide as necessary.


: holds  ( c-addr u -- )  holdptr over - swap cmove ;

\ As part of a <# #> delimited number-to-string conversion, pre-
\ pend the string represented by c-addr u to the string being
\ built.

\ An ambiguous condition exists if HOLDS is executed outside of
\ a <# #> delimited conversion.


: hex  ( -- )  #16 base ! ;

\ Set the base (radix) of the number system to 16 (hexadecimal).



\ Helper Numeric String Words


$____ value holdptr  ( -- c-addr )

\ c-addr is the address of the character in front of the string
\ being built by a <# #> delimited number conversion expression.
\ The next character prepended to the string (e.g. by HOLD) will
\ be written to c-addr.


$____ constant holdbuf  ( -- c-addr )

\ c-addr is the highest address in the buffer that is used by
\ <# #> delimited number-to-string conversion expressions to
\ hold the string being built. holdptr is set to holdbuf when a
\ conversion is started.



\ Core Text Input Words


: (  ( Int: 'ccc<right-paren>' -- )
     ( Com: 'ccc<right-paren>' -- ) ( Run: -- )
  interpretation
    ')' parse
  compilation
    ')' postpone parse ; immediate

\ Parse ccc delimited by ) (right parenthesis). This causes the
\ outer interpreter to skip past the text enclosed in the paren-
\ theses.

\ Standard Forth description for File version (to be revised):

\ When parsing from a text file, if the end of the parse area is
\ reached before a right parenthesis is found, refill the input
\ buffer from the next line of the file, set >IN to zero, and
\ resume parsing, repeating this process until either a right
\ parenthesis is found or the end of the file is reached.


: source  ( -- c-addr u )  textinbuf #textinbuf ;

\ c-addr is the address of the text input buffer used by the
\ Forth outer interpreter. u is the number of characters in the
\ buffer.


variable >in  ( -- a-addr )  0 >in !

\ a-addr is the address of a cell containing the offset in char-
\ acters from the start of the input buffer to the start of the
\ parse area.


: key  ( -- char )
  begin
    textindev c@
    dup invalidchar? while
    drop
  repeat ;

\ Receive one character char from the text input device. Key-
\ board events that do not correspond to characters in the char-
\ acter set are discarded until a valid character is received,
\ and those events are subsequently unavailable.


: accept  ( c-addr +n1 -- +n2 )
  0 ?do
    key
    dup eol? if leave then
    tuck emit c!+
  loop
  drop ;

\ Receive a string of at most +n1 characters from the text input
\ device, write the string to the memory region with starting
\ address c-addr, and display graphic characters as they are re-
\ ceived. Input terminates when a line terminator character is
\ received. When input terminates, nothing is appended to the
\ string, and the display is maintained.

\ An ambiguous condition exists if +n1 is zero or is greater
\ than +32767.


: char  ( '<spaces>name' -- char )  parse-name drop c@ ;

\ Skip leading spaces and parse name delimited by a space. Put
\ the first character of name on the stack.


: word  ( '<chars>ccc<char>' char -- c-addr )
  >r parse-area r@                ( c-addr1 u1 char R:char )
  :noname literal = ; xt-skip r>  ( c-addr2 u2 char )
  :noname literal <> ; xt-skip    ( c-addr3 u3 )
  advance>in                      ( c-addr3 u3 )
  s"buf swap cmove s"buf ;

\ Skip leading delimiters and parse ccc delimited by char. Write
\ the parsed word to a dedicated buffer as a counted string, and
\ put the address of the counted string on the stack. If the
\ parse area was empty or contained no characters other than the
\ delimiter, the resulting string has zero length. A program may
\ replace characters in the string.

\ An ambiguous condition exists if the length of the parsed
\ string is greater than 65535.



\ Core-Ext Text Input Words


: \  ( Com,Exe: 'ccc<eol>' -- )  0 to #textinbuf ; immediate

\ Standard Forth description (to be revised):

\ Compilation: Perform the execution semantics below.

\ Execution: If BLK contains zero, parse and discard the remain-
\ der of the parse area; otherwise parse and discard the portion
\ of the parse area corresponding to the remainder of the cur-
\ rent line. \ is an immediate word. 


: parse  ( 'ccc<char>' char -- c-addr u )
  parse-area rot                ( c-addr u1 char )
  :noname literal <> ; xt-skip  ( c-addr u )
  advance>in ;

\ Parse ccc delimited by the delimiter char. c-addr is the ad-
\ dress of the parsed string within the input buffer, and u is
\ the string length. If the parse area was empty, the resulting
\ string has zero length.


: parse-name  ( '<spaces>name<space>' -- c-addr u )
  parse-area                  ( c-addr1 u1 )
  ['] space? xt-skip over >r  ( c-addr u2 R:c-addr )
  ['] space? 0= xt-skip       ( c-addr2 u3 R:c-addr )
  advance>in                  ( c-addr2 u3 R:c-addr )
  drop r> tuck - ;

\ Skip leading spaces and parse name delimited by a space.
\ c-addr is the address of the parsed string within the input
\ buffer, and u is the string length. If the parse area was emp-
\ ty or contains only white space, the resulting string has zero
\ length.


0 value source-id  ( -- 0 | -1 | fileid )

\ If the input source is the user input device, put 0 on the
\ stack. If the input source is a string via EVALUATE, put -1
\ on the stack. If the input source is a file, put the file ID
\ on the stack.

\ An ambiguous condition exists if SOURCE-ID is  used when BLK
\ contains a non-zero value.


: save-input  ( -- xn...x1 n )  textinbuf >in @ 2 ;

\ x1 through xn describe the current state of the input source
\ specification for later use by RESTORE-INPUT.


: restore-input  ( xn...x1 n -- flag )
  drop to textinbuf >in ! ;

\ Attempt to restore the input source specification to the state
\ described by x1 through xn. flag is true if the input source
\ specification cannot be so restored.

\ An ambiguous condition exists if the input source represented
\ by the arguments is not the same as the current input source.


: refill  ( -- flag )
  false source-id 0=
  if
    textin?
    if
      textinbuf textinmax accept drop true
    then
  then ;

\ Attempt to fill the text input buffer from the input source.

\ When the input source is the text input device, attempt to re-
\ ceive characters into the text input buffer. If successful,
\ the text input buffer contains the received characters, >IN is
\ zero, and flag is true. Receipt of a line containing no char-
\ acters is considered successful. If no input is available from
\ the text input device, flag is false.

\ When the input source is a string via EVALUATE, flag is false
\ and no other action is performed.

\ When the input source is a block, make the next block the in-
\ put source and current input buffer by adding one to the value
\ of BLK and setting >IN to zero. If the new value of BLK is a
\ valid block number, flag is true. Otherwise flag is false.

\ Standard Forth description for File version (to be revised):

\ When the input source is a text file, attempt to read the next
\ line from the text-input file. If successful, make the result
\ the current input buffer, set >IN to zero, and return true.
\ Otherwise return false.



\ Helper Text Input Words


$____ value textinbuf  ( -- c-addr )

\ c-addr is the address of the text input buffer used by the
\ Forth outer interpreter.


0 value #textinbuf  ( -- u )

\ u is the number of characters in the text input buffer used by
\ the Forth outer interpreter.


$____ constant textindev  ( -- c-addr )

\ c-addr is the address of the text input device, which is typi-
\ cally a keybord or a serial port.


$____ constant keydev  ( -- a-addr )

\ c-addr is the address of the hardware register that contains a
\ flag indicating if a character is available.


: invalidchar?  ( char -- flag )
  bl s" MAX_CHAR" environment? within ;

\ If char is a valid character in the character set, flag is
\ true. Otherwise flag is false.


: space?  ( char -- flag )  bl 1+ u< ;

\ If char has a character code less than or equal to that of the
\ space character, flag is true. Otherwise flag is false.


: eol?  ( char -- flag )  $000a = ;

\ If char is a line terminator, flag is true. Otherwise flag is
\ false

\ In Opforth, the line terminator character is ASCII $0a (new
\ line), and the size of a character is 16 bits.


: hex?  ( char -- flag )
  dup '0' '9' 1+ within >r
  dup 'A' 'F' 1+ within >r
  dup 'a' 'f' 1+ within
  r> or r> or ;

\ If char is a valid hexadecimal digit, flag is true. Otherwise
\ flag is false.


: xt-skip  ( c-addr1 u1 xt -- c-addr2 u2 )
  >r
  begin
    dup while
    over c@ r@ execute while
    1 /string
  repeat then
  r> drop ;

\ Adjust the string with address c-addr1 and length u1 by skip-
\ ping all leading characters satisfying xt ( char -- flag ).
\ c-addr2 is the address of the first character that causes xt
\ to return a false flag, and u2 is the length of the adjusted
\ string.


: parse-area  ( -- c-addr u )  source >in @ /string ;

\ c-addr is the address of the parse area pointed to by >IN. u
\ is the number of characters in the parse area.


: advance>in  ( c-addr u -- c-addr u )
  2dup 1 min + textinbuf = >in ! ;

\ Given a string that has starting address c-addr within the in-
\ put buffer and length u, set >IN to point to the first charac-
\ ter past the end of the string.



\ Core Query Words


: depth  ( -- +n )  sp0 sp@ - ;

\ +n is the number of single-cell items contained in the data
\ stack before +n was placed on the stack.


: environment?  ( c-addr u -- false | i*x true )
  0
  case
  [ s" /COUNTED-STRING" ] compare of $fffe endof
  [ s" /HOLD " ] compare of $0100 endof
  [ s" /PAD " ] compare of $0100 endof
  [ s" ADDRESS-UNIT-BITS" ] compare of 8 endof
  [ s" FLOORED" ] compare of false endof
  [ s" MAX-CHAR" ] compare of $7e endof
  [ s" MAX-D" ] compare of $efffffff. endof
  [ s" MAX-N" ] compare of $efff endof
  [ s" MAX-U" ] compare of $ffff endof
  [ s" MAX-UD" ] compare of $ffffffff. endof
  [ s" RETURN-STACK-CELLS" ] compare of $1000 endof
  [ s" STACK-CELLS" ] compare of $1000 endof
  endcase
  0<> if true then ;

\ Provide information about the programming environment by at-
\ tempting to match the string with address c-addr and length u
\ to an entry in the table below. If no match is found, return
\ a false flag. Otherwise, return the query result i*x with a
\ true flag.

\ String              Type  Meaning

\ /COUNTED-STRING     n     Counted string length limit in chars
\ /HOLD               n     Size of the buffer used by <# #>
\ /PAD                n     Size of the buffer used by PAD
\ ADDRESS-UNIT-BITS   n     Size of one address unit in bits
\ FLOORED             flag  true if division is floored
\ MAX-CHAR            u     Maximum value of any character
\ MAX-D               d     Maximum signed double-cell integer
\ MAX-N               n     Maximum signed single-cell integer
\ MAX-U               u     Maximum unsigned single-cell integer
\ MAX-UD              ud    Maximum unsigned double-cell integer
\ RETURN-STACK-CELLS  n     Maximum cells on the return stack
\ STACK-CELLS         n     Maximum cells on the data stack



\ Core-Ext Query Words


: unused  ( -- u )  dpmax dp @ - ;

\ u is the number of address units remaining in the space ad-
\ dressed by HERE.

\ In Opforth, there is no distinction between address units and
\ characters.



\ Core Execution Token Words


$____ opcode execute  ( i*x xt -- j*x )

\ Remove xt from the stack and execute the word corresponding to
\ xt.


: '  ( '<spaces>name' -- xt )
  bl word find 0<> if drop ( error-code ) throw then ;

\ Skip leading spaces and parse name delimited by a space. Find
\ name and put the corresponding execution token on the stack.
\ When interpreting, ' xyz EXECUTE is equivalent to xyz.


: find  ( c-addr -- c-addr 0 | xt 1 | xt -1 )
  dlink >r                          \ Get dictionary head link
  begin
    r@ 0= if 0 rdrop exit then      \ Exit if end of list
    r@ cell+                        \ Get name field address
    dup @ length-mask and >r        \ Get name length
    over c@ r@ =                    \ Test if lengths match
    if
      over count third cell+ r@     \ Put both strings on stack
      compare 0=                    \ Test if strings match
      if
        nip dup r> + swap @         \ Get data field address
        swap @ flags-mask           \ Get flags
        dup combined-flag and       \ Test if combined word
        if
          swap dup @ state @ and    \ Get offset for xt address
          + 1+ swap 1               \ Add offset, put 1 on stack
        else
          dup immediate-flag and    \ Get immediate flag
          0<> 1 or                  \ Put 1 or -1 or the stack
        then
        rdrop exit                  \ Drop name length and exit
      then
    then
    drop rdrop                      \ Drop name addr and length
  again ;

\ Determine if the counted string at c-addr matches the name of
\ any existing dictionary definition. If no match is found, re-
\ turn c-addr and zero. If a match is found, return the corre-
\ sponding execution token. If the definition is immediate, also
\ return 1. Otherwise, also return -1. For a given string, the
\ results returned by FIND while compiling may differ from those
\ returned while not compiling.


: >body  ( xt -- a-addr )
  create-flag and if 3 cells + else ( error-code ) throw then ;

\ a-addr is the data field addressof the definition with execu-
\ tion token xt.

\ In Opforth, the lower cell of the execution token is the ad-
\ dress of the data field.

\ If xt is not the execution token of a word defined by CREATE,
\ ( throw an error ).



\ Core-Ext Execution Token Words


: defer@  ( xt1 -- xt2 )
  defer-flag ( test if xt1 has been set ) or and if
  cell+ @ else ( error-code ) throw then ;

\ xt2 is the execution token xt1 is set to execute.

\ If xt1 is not the execution token of a word defined by DEFER,
\ or if xt1 has not been set to execute an xt, ( throw an
\ error ).


: defer!  ( xt1 xt2 -- )
  defer-flag and if
  swap cell+ ! else ( error-code ) throw then ;

\ Set the word xt1 to execute xt2.

\ If xt1 is not for a word defined by DEFER, ( throw an error ).


: is  ( Int: '<spaces>name' xt -- )
      ( Com: '<spaces>name' -- ) ( Run: xt -- )
  interpretation
    defer-flag and if
    ' cell+ ! else
    ( error-code ) throw then
  compilation
    defer-flag and if
    ' cell+ literal postpone ! else
    ( error-code ) throw then ; immediate

\ Interpretation: Skip leading spaces and parse name delimited
\ by a space. Set name to execute xt.

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Compile the runtime semantics described below.

\ Runtime: Set name to execute xt.

\ If name was not defined by DEFER, ( throw an error ).

\ An ambiguous condition exists if POSTPONE, [COMPILE], ['], or
\ ' is applied to IS.


: action-of  ( Int: '<spaces>name' -- xt )
             ( Com: '<spaces>name' -- ) ( Run: -- xt )
  interpretation
    defer-flag ( test if xt has been set ) or and if
    ' cell+ @ else
    ( error-code ) throw then
  compilation
    defer-flag ( test if xt has been set ) or and if
    ' cell+ literal postpone @ else
    ( error-code ) throw then ; immediate

\ Interpretation: Skip leading spaces and parse name delimited
\ by a space. xt is the execution token that name is set to exe-
\ cute.

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Compile the runtime semantics described below.

\ Runtime: xt is the execution token that name is set to exe-
\ cute.

\ If name was not defined by DEFER, or if name was not set to
\ execute an xt, ( throw an error ).

\ An ambiguous condition exists if POSTPONE, [COMPILE], ['], or
\ ' is applied to ACTION-OF.



\ Helper Execution Token Words


: interpretation  ( Com: -- a-addr )
  deflink dup @ combined-flag or swap !  \ Set combined flag
  postpone branch                        \ Compile a branch
  here 0 , ; immediate compile-only      \ Push branch field

\ Make the current definition a combined word by setting the
\ combined flag and compiling a branch to the compilation seman-
\ tics. a-addr is the address of the branch field, which will be
\ resolved by COMPILATION.

\ The words compiled into the body of the definition after
\ INTERPRETATION are the interpretation semantics of the combin-
\ ed word.


: compilation  ( Com: a-addr -- )
  postpone exit                         \ Compile an exit
  here swap ! ; immediate compile-only  \ Resolve branch field

\ Inside the definition of a combined word, end the interpreta-
\ tion section by compiling an exit and resolving the branch
\ field that was compiled by INTERPRETATION.

\ The words compiled into the body of the definition after
\ COMPILATION are the compilation semantics of the combined
\ word.



\ Core Compiler Words


: here  ( -- addr )  dp @ ;

\ addr is the dictionary pointer.


: ,  ( x -- )  cell allot here ! ;

\ Reserve one cell of dictionary space and store x in the cell.
\ If the dictionary pointer is aligned when , begins execution,
\ it will remain aligned when , finishes execution.

\ In Opforth, all addresses are aligned addresses. This imple-
\ mentation of , is compatible with Forth systems that may have
\ non-aligned addresses.

\ An ambiguous condition exists if the dictionary pointer is not
\ aligned prior to the execution of ,.


: c,  ( char -- )  1 chars allot here c! ;

\ Reserve space for one character in the dictionary and store
\ char in the space. If the dictionary pointer is character-
\ aligned when C, begins execution, it will remain character-
\ aligned when C, finishes execution.

\ In Opforth, the size of a cell is two characters, there is no
\ distinction between address units and characters, and all ad-
\ dresses are character-aligned.


: allot  ( n -- )  dp @ tuck + dp ! ;

\ If n is greater than zero, reserve n address units of dictio-
\ nary space. If n is less than zero, release |n| address units
\ of dictionary space. If n is zero, leave the dictionary point-
\ er unchanged. If the dictionary pointer is aligned and n is a
\ multiple of the size of a cell when ALLOT begins execution, it
\ will remain aligned when ALLOT finishes execution.


: align  ( -- )  here odd 1 and + ;

\ If the dictionary pointer is not aligned, reserve enough space
\ to align it.

\ In Opforth, the size of a cell is two characters, and there is
\ no distinction between characters and address units.


: [  ( -- )  false state ! ; immediate

\ Enter interpretation state.


: ]  ( -- )  true state ! ;

\ Enter compilation state.


variable state  ( -- a-addr )  false state !

\ a-addr is the address of a cell containing the compilation-
\ state flag. STATE is true when in compilation state. STATE is
\ false otherwise. Only the following standard words alter the
\ value of STATE: : (colon), ; (semicolon), ABORT, QUIT,
\ :NONAME, [ (left-bracket), ] (right-bracket).


: postpone  ( Com: '<spaces>name' -- )
  state @ ] ['] ' execute if
  drop , state ! else ( error-code ) throw then ;

\ Skip leading spaces and parse name delimited by a space. Find
\ name in the dictionary. Compile the compilation semantics of
\ name.

\ If name is not found, ( throw an error ).


: literal  ( Com: x -- ) ( Run: -- x )
  postpone lit
  , ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the following runtime semantics.

\ Runtime: Put x on the stack.


: [char]  ( Com: '<spaces>name' -- ) ( Run: -- char )
  parse-name drop c@ literal ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Compile the following runtime semantics.

\ Runtime: Put the value of the first character of name on the
\ stack.


: [']  ( Com: '<spaces>name' -- ) ( Run: -- xt )
  [ ' ] 2literal ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Find name in the dictionary. Compile the following run-
\ time semantics.

\ Runtime: Put the execution token corresponding to name onto
\ the stack. The execution token returned by the compiled phrase
\ "['] X" is the same token returned by "' X" outside of compi-
\ lation state.

\ An ambiguous condition exists if name is not found.


: s"  ( Int: 'ccc<quote>' -- c-addr u )
      ( Com: 'ccc<quote>' -- ) ( Run: -- c-addr u )
  interpretation
    '"' parse
    2dup s"buf swap cmove
  compilation
    '"' parse postpone sliteral ; immediate

\ Interpretation: Parse ccc delimited by " (double-quote). Store
\ the resulting string in a transient buffer. c-addr is string
\ address, and u is the string length.

\ Compilation: Parse ccc delimited by " (double-quote). Compile
\ the following runtime semantics.

\ Runtime: Return the address c-addr and length u of a string
\ consisting of the characters ccc. A program shall not alter
\ the returned string.



\ Core-Ext Compiler Words


: s\"  ( Int: 'ccc<quote>' -- c-addr u )
        ( Com: 'ccc<quote>' -- ) ( Run: -- c-addr u )
  interpretation
    s\"buf to s\"ptr parse-area
    begin
      dup 0<> while
      /char '"' <> while
      over c@ dup '\'
      if
        over 0<>
        if
          drop /char
          case
          'x' of
            dup 2 u< while
            foo
          endof
          'a' of $07 endof
          'b' of $08 endof
          'e' of $1b endof
          'f' of $0c endof
          'l' of $0a endof
          'm' of $0d s\"append $0a endof
          'n' of $0a endof
          'q' of $22 endof
          'r' of $0d endof
          't' of $09 endof
          'v' of $0b endof
          'z' of $00 endof
          '\' of $5c endof
          '"' of $22 endof
          ( default ) third char- c@ s\"append
          endcase
        then
      then
      s\"append
    repeat then then
  compilation
    [ s\" ] postpone sliteral ; immediate  \ not findable yet?

\ Interpretation: Undefined

\ Compilation: Parse ccc delimited by " (double-quote) using the
\ translation rules defined below. Compile the following runtime
\ semantics.

\ Runtime: Return the address c-addr and length u of a string
\ consisting of the characters ccc. A program shall not alter
\ the returned string.

\ Translation Rules: Characters are processed one at a time and
\ appended to the compiled string. If the character is \ (back-
\ slash), it is processed by parsing and substituting one or
\ more characters according to the table below. The character
\ after the backslash is case sensitive.

\ Characters        Meaning                    ASCII Code (Hex)

\ \a                alert                      07
\ \b                backspace                  08
\ \e                escape                     1b
\ \f                form feed                  0c
\ \l                line feed                  0a
\ \m                carriage return line feed  0d 0a
\ \n                new line                   0a
\ \q                double quote               22
\ \r                carriage return            0d
\ \t                horizontal tab             09
\ \v                vertical tab               0b
\ \z                no character               00
\ \"                double quote               22
\ \x<digit><digit>  ASCII character by code    <digit><digit>
\ \\                backslash itself           5c

\ Opforth translates \n to the ASCII line feed code, but other
\ Forth systems may translate it differently.


: c"  ( Com: 'ccc<quote>' -- ) ( Run: -- c-addr )
  '"' word ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Parse ccc delimited by " (double-quote) and com-
\ pile the following runtime semantics.

\ Runtime: Return the address of a counted string consisting of
\ the characters ccc. A program shall not alter the returned
\ string.


: compile,  ( Com: -- ) ( Exe: xt -- )
  postpone drop
  postpone , ; immediate compile-only

\ Interpretation: Undefined

\ Execution: Compile the execution semantics of the definition
\ represented by xt.


: [compile]  ( Com: '<spaces>name' -- )
  ' if drop , else ( error-code ) then ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Find name in the dictionary. If name has other than de-
\ fault compilation semantics, compile the compilation seman-
\ tics of name. Otherwise, compile the execution semantics of
\ name.

\ If name is not found, ( throw an error ).

\ Note: This word is obsolescent and is included for compatibil-
\ ity with existing Forth code.



\ Helper Compiler Words


$____ opcode lit  ( -- x )

\ Put x, the contents of the next consecutive cell of memory,
\ onto the stack.


: 2,  ( x1 x2 -- )  swap , , ;

\ Reserve two cells of dictionary space. Store x1 to the first
\ cell, and store x2 to the second cell. If the dictionary
\ pointer is aligned when 2, begins execution, it will remain
\ aligned when 2, finishes execution.

\ In Opforth, the size of a cell is two characters.

\ An ambiguous condition exists if the dictionary pointer is not
\ aligned prior to the execution of 2,.


variable dp  ( -- a-addr )  $____ dp !

\ a-addr is the address of a cell containing the dictionary
\ pointer.


$____ value dpmax  ( -- a-addr )

\ a-addr is the highest address that is usable by the dictionary
\ pointer.


$____ constant s"buf  ( -- c-addr )

\ c-addr is the address of the buffer used by S".


$____ constant s\"buf  ( -- c-addr )

\ c-addr is the address of the buffer used by S\".


$____ value s\"ptr  ( -- c-addr )

\ c-addr is the address of the next character to be written to
\ S\"BUF.


: s\"append  ( char -- )  s\"ptr c!+ to s\"ptr ;

\ Append char to the buffer used by S\".


: \x  ( c-addr1 u1 -- c-addr2 u2 | c-addr2 u2 char )
  /char dup hex?
  if
    drop over c@ dup hex? if
    drop over 0 swap 0 swap 2 >number 2drop drop then
  else
    s\"append /char
  then ;

\ If the first two characters of the string with address c-addr1
\ and length u1 are valid hexadecimal digits, convert the digits
\ to the character code with the same value,

\ If both characters are valid hex digits, put the character
\ with the equivalent character code on the stack.

\ If the first character is not a valid hex digit, append both
\ characters.

\ If the first character is a valid hex digit, but the second
\ character is not, append the second character.

\ c-addr2 is the address 2 characters past c-addr1, and u2 is
\ u1 - 2.



\ Core Definition Words


: :  ( '<spaces>name' -- flag )
     ( Ini: i*x R: -- i*x R:a-addr ) ( Exe: i*x -- j*x )
  0 define true    \ Compile header, push findable flag
  here to defaddr  \ Save code field address
  ] ;              \ Enter compilation state

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a new definition for name. Enter compilation state, start
\ the current definition, and produce colon-sys. Compile the ex-
\ ecution semantics described below. The execution semantics
\ will be determined by the words compiled into the body of the
\ definition. The current definition will not be findable in the
\ dictionary until it is ended.

\ Initiation: Put the return address nest-sys on the return
\ stack. The stack effects i*x represent arguments to name.

\ name Execution: Execute the definition. The stack effects i*x
\ and j*x represent arguments to and results from name, respect-
\ ively.


: ;  ( Com: flag -- ) ( Run: R:a-addr -- R: )
  postpone exit if
  findable then [ ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below.
\ End the current definition, allow it to be found in the dic-
\ tionary, enter interpretation state, and consume colon-sys.
\ If the dictionary pointer is not aligned, reserve enough space
\ to align it.

\ Runtime: Return to the calling definition specified by the re-
\ turn address nest-sys.


: immediate  ( -- )
  deflink cell+ dup @ immediate-flag or swap ! ;

\ Make the most recent definition an immediate word.

\ An ambiguous condition exists if the most recent definition
\ does not have a name or if it was defined by SYNONYM.


: constant  ( '<spaces>name' x -- ) ( Exe: -- x )
  create , does> @ ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the following execution seman-
\ tics.

\ name Execution: Put x on the stack.


: variable  ( '<spaces>name' -- ) ( Exe: -- a-addr )
  create 0 , ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the following execution seman-
\ tics.

\ name Execution: a-addr is the address of the reserved cell. A
\ program is responsible for initializing the contents of the
\ reserved cell.


: create  ( '<spaces>name' -- ) ( Exe: -- a-addr )
  create-flag define
  here 3 cells + literal
  here to defaddr postpone exit ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below. If the dictionary pointer is not aligned, re-
\ serve enough space to align it. The new dictionary pointer de-
\ fines the data field of name. CREATE does not allocate dictio-
\ nary space in the data field.

\ Execution: a-addr is the address of the data field of name.
\ The execution semantics of name may be extended by using
\ DOES>.


: does>  ( Com: flag1 -- flag2 )
         ( Run: R:nest-sys1 -- R: )
         ( Ini: i*x R: -- i*x a-addr R:nest-sys2 )
         ( Exe: i*x -- j*x )
  here defaddr ! postpone rdrop ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below.
\ Consume colon-sys1 and produce colon-sys2. Compile the initia-
\ tion semantics described below. Standard Forth does not re-
\ quire the compilation of DOES> to make the current definition
\ findable in the dictionary.

\ Runtime: Replace the execution semantics of the most recent
\ definition, referred to as name, with the name execution se-
\ mantics described below. Return control to the calling defini-
\ tion specified by nest-sys1.

\ Initiation: Put the return address nest-sys2 on the return
\ stack. Put the address of the data field of name on the data
\ stack. The stack effects i*x represent arguments to name.

\ name Execution: Execute the portion of the definition that be-
\ gins with the initiation semantics appended by the DOES> that
\ modified name. The stack effects i*x and j*x represent argu-
\ ments to and results from name, respectively.

\ An ambiguous condition exists if name was not defined by
\ CREATE or a user-defined word that calls CREATE.



\ Core-Ext Definition Words


: :noname  ( -- xt flag )
           ( Ini: i*x R: -- i*x R:a-addr ) ( Exe: i*x -- j*x )
  here 0 false ] ;

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
  create cells allot ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below. Reserve u address units at an aligned address.

\ The area reserved by the Opforth implementation of BUFFER: is
\ contiguous with the rest of the dictionary. Standard Forth
\ does not require the reserved area to be contiguous.

\ name Execution: a-addr is the address of the space reserved by
\ BUFFER: when it defined name. A program is responsible for
\ initializing the contents.


: value  ( '<spaces>name' x -- ) ( Exe: -- x ) ( To: x -- )
  create , does> @ ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below. Reserve one cell of dictionary space and write
\ x to the cell.

\ name Execution: Put x on the stack. The contents of x are
\ those assigned by the most recent execution of the phrase
\ x TO name. If x TO name has not been executed, the contents
\ of x are those assigned when name was created.

\ TO name Runtime: Write x to the data field of name.


: to  ( Int: '<spaces>name' i*x -- )
      ( Com: '<spaces>name' -- )
  interpretation
    ' dup value-flag and if cell+ ! then
    2value-flag and if cell+ swap 2! then
  compilation
    ' dup value-flag and
    if
      postpone cell+
      postpone !
    then
    2value-flag and
    if
      postpone cell+
      postpone swap
      postpone 2!
    then ; immediate

\ Interpretation: Skip leading spaces and parse name delimited
\ by a space. Perform the "TO name Runtime" semantics given in
\ the definition of the defining word of name.

\ Compilation: Skip leading spaces and parse name delimited by a
\ space. Compile the "TO name Runtime" semantics given in the
\ definition of the defining word of name.

\ An ambiguous condition exists if name was not defined by a
\ word with "TO name Runtime" semantics, or if any of POSTPONE,
\ [COMPILE], or ', or ['] are applied to TO.


: defer  ( '<spaces>name' -- ) ( Exe: i*x -- j*x )
  defer-flag define postpone branch
  0 , ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below.

\ name Execution: Execute the xt that name is set to execute.

\ An ambuguous condition exists if name has not been set to exe-
\ cute an xt.


: marker  ( '<spaces>name' -- ) ( Exe: -- )
  create
    dlink , here ,
  does>
    dup cell+ @ dp !
    @ to dlink ;

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



\ Helper Definition Words


: define  ( '<spaces>name<space>' x -- )
  parse-name
  here to deflink
  dlink , third over or , string,
  align ;

\ Skip leading spaces and parse name delimited by a space. Com-
\ pile a dictionary header for a new dictionary definition using
\ the cell x as the flags and the parsed name as the name.


: opcode  ( '<spaces>name<space>' x -- )  0 define , ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a dictionary definition for built-in machine language in-
\ struction.


: compile-only  ( -- )
  deflink cell+ dup @ compile-only-flag or swap ! ;

\ Make the most recent definition a compile-only word, which is
\ a word that will cause the outer interpreter to abort and dis-
\ play an error message if an attempt is made to execute the
\ word in interpretation state.


: findable  ( -- )  deflink to dlink ;

\ Put the most recent definition at the front of the search or-
\ der.


0 value deflink  ( - a-addr )

\ a-addr is the link field address of the most recent defini-
\ tion. DEFLINK is used as temporary storage when a definition
\ has been started but has not been made findable yet.


0 value dlink  ( -- a-addr )

\ a-addr is the head link of the linked list used to find words
\ in the dictionary.


0 value defaddr  ( -- a-addr )

\ If the current definition is being defined by : or :NONAME,
\ a-addr is the starting address of the code within the defini-
\ tion so it RECURSE can compile a branch to it.

\ If the current definition is being defined by CREATE, a-addr
\ is the address of the EXIT so DOES> can overwrite it.


$____ constant length-mask  ( -- x )

\ x is a bit field that can mask the name length cell of a dic-
\ tionary definition to obtain the length without the flags.


$____ constant flags-mask  ( -- x )

\ x is a bit field that can mask the name length cell of a dic-
\ tionary definition to obtain the flags without the length.


$____ constant immediate-flag  ( -- x )

\ x is a cell with the bit position of the flag for immediate
\ words set.


$____ constant compile-only-flag  ( -- x )

\ x is a cell with the bit position of the flag for compile-only
\ words set.

$____ constant combined-flag  ( -- x )

\ x is a cell with the bit position of the flag for combined
\ words set.


$____ constant create-flag  ( -- x )

\ x is a cell with the bit position of the flag for words defin-
\ ed by CREATE set.


$____ constant defer-flag  ( -- x )

\ x is a cell with the bit position of the flag for words defin-
\ ed by DEFER set.


$____ constant value-flag  ( -- x )

\ x is a cell with the bit position of the flag for words defin-
\ ed by VALUE set.


$____ constant 2value-flag  ( -- x )

\ x is a cell with the bit position of the flag for words defin-
\ ed by 2VALUE set.



\ Core Control Flow Words


: if  ( Com: -- orig ) ( Run: x -- )
  postpone ?branch
  here 0 , ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Put the location of a new unresolved forward ref-
\ erence orig onto the stack. Compile the runtime semantics de-
\ scribed below. The semantics are incomplete until resolved,
\ e.g., by THEN or ELSE.

\ Runtime: If all bits of x are zero, continue execution at the
\ location specified by the resolution of orig.


: then  ( Com: orig -- ) ( Run: -- )
  here swap ! ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below.
\ Resolve the forward reference orig using the location of the
\ compiled runtime semantics.

\ Runtime: Continue execution.


: else  ( Com: orig1 -- orig2 ) ( Run: -- )
  postpone then
  postpone branch
  here 0 , ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Put the location of a new unresolved forward ref-
\ erence orig2 onto the stack. Compile the runtime semantics de-
\ scribed below. The semantics will be incomplete until resolved
\ (e.g. by THEN). Resolve the forward reference orig1 using the
\ location of the compiled runtime semantics.

\ Runtime: Continue execution at the location given by the res-
\ olution of orig2.


: begin  ( Com: -- dest ) ( Run: -- )
  here ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Put dest, the next location for a transfer of
\ control, onto the stack. Compile the runtime semantics below.

\ Runtime: Continue execution.


: until  ( Com: dest -- ) ( Run: x -- )
  postpone ?branch
  , ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below.
\ Resolve the backward reference dest.

\ Runtime: If all bits of x are zero, continue execution at the
\ location specified by dest.


: while  ( Com: dest -- orig dest ) ( Run: x -- )
  postpone if
  swap ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Put orig, the location of a new unresolved for-
\ ward reference, onto the stack under the existing dest. Com-
\ pile the runtime semantics described below. The semantics are
\ incomplete until orig and dest are resolved (e.g., by REPEAT).

\ Runtime: If all bits of x are zero, continue execution at the
\ location specified by the resolution of orig.


: repeat  ( Com: orig dest -- ) ( Run: -- )
  postpone again
  postpone then ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below and
\ resolve the backward reference dest. Resolve the forward ref-
\ erence orig using the location following the compiled runtime
\ semantics.

\ Runtime: Continue execution at the location given by dest.


: do  ( Com: -- false a-addr )
      ( Run: n1|u1 n2|u2 R: -- R:n1|u1 R:n2|u2 )
  0 postpone 2>r
  here ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Put a copy of the dictionary pointer onto the
\ data stack. Compile the runtime semantics below. The seman-
\ tics are incomplete until resolved by LOOP or +LOOP.

\ Runtime: Set up loop parameters by transferring the top two
\ data stack items to the return stack. The top item n1|u1 is
\ the loop index and the second item n2|u2 is the loop limit.

\ Standard Forth does not allow a program to access any return
\ stack items underneath the loop parameters until the parame-
\ ters are discarded.

\ An ambiguous condition exists if the index and limit are not
\ of the same type.


: loop  ( Com: flag a-addr -- )
        ( Run: R:n1|u1 R:n2|u2 -- R:|n1|u1 R:|n3|u3 )
  postpone r>
  postpone 1+
  postpone ?loop
  , dup if here swap ! else drop then
  resolve-leave ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics below. Resolve the
\ destination of all unresolved occurrences of LEAVE - between
\ the location given by do-sys and the next location for a
\ transfer of control - to execute the words following the LOOP.

\ Runtime: Add one to the loop index. If the loop index is then
\ equal to the loop limit, discard the loop parameters and con-
\ tinue execution immediately following the loop. Otherwise,
\ continue execution at the beginning of the loop.

\ An ambiguous condition exists if the runtime semantics execute
\ when the loop parameters are unavailable.


: +loop  ( Com: flag a-addr -- )
         ( Run: n R:n1|u1 R:n2|u2 -- R:|n1|u1 R:|n3|u3 )
  postpone r@+
  postpone r>
  postpone r@
  postpone third
  postpone >r
  postpone within
  postpone 0=
  postpone ?branch
  , dup if here swap ! else drop then
  resolve-leave ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below.
\ Resolve the destination of all unresolved occurrences of LEAVE
\ - between the location of do-sys and the next location for a
\ transfer of control - to execute the words following +LOOP.

\ Runtime: Add n to the loop index. If the loop index did not
\ cross the boundary between the loop index minus one and the
\ loop limit, continue execution at the beginning of the loop.
\ Otherwise, discard the loop parameters and continue execution
\ immediately following the loop.

\ An ambiguous condition exists if the runtime semantics execute
\ when the loop parameters are unavailable.


synonym i r@  ( Com: -- ) ( Exe: R:n|u -- n|u R:n|u )

\ Interpretation: Undefined

\ Execution: Put a copy of the current (innermost) loop index
\ onto the data stack. The loop index can be signed or unsigned.

\ An ambiguous condition exists if the loop parameters are un-
\ available.


: j  ( Com: -- )  ( Exe: R:n1|u1 R:n2|u2 R:n3|u3 --
                         n3|u3 R:n1|u1 R:n2|u2 R:n3|u3 )
  postpone r>
  postpone r>
  postpone r@
  postpone swap>r
  postpone swap>r ; immediate compile-only

\ Interpretation: Undefined

\ Execution: Put a copy of the next-outer loop index onto the
\ data stack. The loop index can be signed or unsigned. An am-
\ biguous condition exists if the loop parameters of the next-
\ outer loop, loop-sys2, are unavailable.


: leave  ( Com: -- ) ( Exe: R:n1|u1 R:n2|u2 -- R: )
  postpone unloop
  postpone branch
  here leave-link , to leave-link ; immediate compile-only

\ Interpretation: Undefined

\ Execution: Discard the current loop parameters. Continue exe-
\ cution immediately following the innermost syntactically en-
\ closing DO...LOOP or DO...+LOOP.

\ An ambiguous condition exists if they are unavailable.


: unloop  ( Com: -- ) ( Exe: R:n1|u1 R:n2|u2 -- R: )
  rdrop rdrop ; immediate compile-only

\ Interpretation: Undefined

\ Execution: Discard the loop parameters for the current nesting
\ level. An UNLOOP is required for each nesting level before the
\ definition may be EXITed.

\ An ambiguous condition exists if the loop parameters are un-
\ available.


$____ opcode exit  ( Com: -- ) ( Exe: R:a-addr -- R: )
  compile-only

\ Interpretation: Undefined

\ Execution: Return control the calling definition specified by
\ the return address nest-sys. Before executing EXIT within a
\ counted loop (e.g. DO...LOOP), a program shall discard the
\ loop parameters by executing UNLOOP.


: recurse  ( Com: -- )  defaddr compile, ;

\ Interpretation: Undefined

\ Compilation: Compile the execution semantics of the current
\ definition.

\ An ambiguous condition exists if RECURSE appears in a defini-
\ tion after DOES>.



\ Core-Ext Control Flow


: again  ( Com: dest -- ) ( Run: -- )
  postpone branch
  , ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the runtime semantics described below and
\ resolve the backward reference dest.

\ Runtime: Continue execution at the location specified by dest.
\ If no other control flow words are used, any program code af-
\ ter AGAIN will not be executed.


: ?do  ( Com: -- true a-addr )
       ( Run: n1|u1 n2|u2 R: -- R:|n1|u1 R:|n2|u2 )
  postpone 2dup
  postpone =
  postpone if
  postpone 2>r
  here ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Put a copy of the dictionary pointer onto the
\ data stack. Compile the runtime semantics described below. The
\ semantics are incomplete until resolved by LOOP or +LOOP.

\ Runtime: If n1 is bit-for-bit the same as n2, skip over the
\ ?DO...LOOP or ?DO...+LOOP control structure and continue exe-
\ cution at the location following the loop. Otherwise, set up
\ loop parameters by transferring the top two data stack items
\ to the return stack, and then continue execution inside the
\ loop. The top item n1|u1 is the loop index and the second item
\ n2|u2 is the loop limit.

\ Standard Forth does not allow a program to access any return
\ stack items underneath the loop parameters until the parame-
\ ters are discarded.

\ An ambiguous condition exists if the index and limit are not
\ of the same type.


: case  ( Com: -- case-sys ) ( Run: -- )
  0 ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Mark the start of the CASE...OF...ENDOF...ENDCASE
\ structure. Compile the following runtime semantics.

\ Runtime: Continue execution.


: of  ( Com: -- of-sys ) ( Run: x1 x2 -- |x1 )
  postpone over
  postpone =
  postpone ?branch
  here 0 , postpone drop ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Put of-sys on the stack and compile the runtime
\ semantics described below. The semantics are incomplete until
\ resolved by a consumer of of-sys such as ENDOF.

\ Runtime: If the top two stack items are not bit-for-bit the
\ same, discard the top item and continue execution at the loca-
\ tion specified by the consumer of of-sys (e.g., following the
\ next ENDOF). Otherwise, discard both values and continue exe-
\ cution in line.


: endof  ( Com: case-sys1 of-sys -- case-sys2 ) ( Run: -- )
  postpone branch
  here rot , here rot ! ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Mark the end of the OF...ENDOF part of the CASE
\ structure. The next location for a transfer of control re-
\ solves the reference given by of-sys. Compile the runtime se-
\ mantics described below. Replace case-sys1 with case-sys2,
\ which will be resolved by ENDCASE.

\ Runtime: Continue execution at the location specified by the
\ consumer of case-sys2.


: endcase  ( Com: case-sys -- ) ( Run: x -- )
  here >r
  begin
    dup while
    dup @ r@ rot !
  repeat
  drop rdrop postpone drop ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Mark the end of the CASE...OF...ENDOF...ENDCASE
\ structure. Use case-sys to resolve the entire structure. Com-
\ pile the following runtime semantics.

\ Runtime: Discard the case selector x and continue execution.



\ Helper Control Flow Words


$____ opcode nop  ( Com: -- ) ( Exe: -- )
  compile-only

\ Do nothing (i.e. no operation).


$____ opcode branch  ( Com: -- ) ( Exe: -- )
  compile-only

\ Continue execution at the address contained in the next con-
\ secutive cell after the BRANCH opcode.


$____ opcode ?branch  ( Com: -- ) ( Exe: x -- )
  compile-only

\ If all bits of x are zero, continue execution at the address
\ contained in the next consecutive cell after the ?BRANCH op-
\ code. Otherwise, continue execution in line.


$____ opcode ?loop  ( Com: -- )
                    ( Exe: n1 R:n2|u -- | R:n2|u R:n1 )
  compile-only

\ Interpretation: Undefined

\ Execution: If the top data stack item n1 is less than the top
\ return stack item, transfer n1 to the return stack and contin-
\ ue execution at the address contained in the next consecutive
\ cell after the ?LOOP opcode. Otherwise, remove both the top
\ data stack item and the top return stack item, and then con-
\ tinue execution in line.


0 value leave-link  ( -- a-addr )

\ a-addr is the address of the head link of the linked list of
\ LEAVE branch address fields.


: resolve-leave  ( Com: -- )  ( Exe: -- )
  here leave-link
  begin
    dup while
    2dup ! @
  repeat
  nip to leave-link ; immediate compile-only

\ As part of the compilation of a counted loop, resolve all oc-
\ currences of LEAVE by writing the dictionary pointer to the
\ branch address fields compiled by the occurrences of LEAVE.



\ Core Outer Interpreter Words


: quit  ( R:i*x -- R: )
  rp0 rp! 0 to source-id
  begin
    postpone [
    refill while
    ['] interpret catch
    case
    0 of state @ 0= if ." ok" then cr endof
    -1 of ( aborted ) endof
    -2 of errorbuf #errorbuf type endof
    ( default ) dup ." Exception # " .
    endcase
  repeat
  bye ;

\ Empty the return stack, store zero in SOURCE-ID, make the user
\ input device the input source, and enter interpretation state.

\ Do not display a message. Repeat the following:
\ - Accept a line from the input source into the input buffer,
\   set >IN to zero, and interpret.
\ - Display "ok" if in interpretation state, all processing has
\   been completed, and no ambiguous condition exists.


: abort  ( i*x R:j*x -- )  -1 throw ;

\ Perform the function of -1 THROW.


: abort"  ( Com: 'ccc<quote>' -- )
          ( Run: i*x x1 R:j*x -- |i*x R:|j*x )
  '"' parse dup to #errorbuf errorbuf swap cmove
  postpone if
  postpone sliteral
  postpone type
  -2 literal postpone throw
  postpone then ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Parse ccc delimited by " (double-quote). Compile
\ the following runtime semantics.

\ Runtime: Write the parsed string to the error message buffer.
\ Remove x1 from the stack. If any bit of x1 is nonzero, perform
\ the function of -2 THROW. If there is no exception frame on
\ the return stack, the error message will be displayed by QUIT.


: evaluate  ( i*x c-addr u -- j*x )
  save-input
  -1 to source-id to #textinbuf to textinbuf 0 >in !
  interpret
  restore-input ;

\ Save the current input source specification, store -1 in
\ SOURCE-ID, store zero in BLK, make the string with address
\ c-addr and length u both the input source and the input buf-
\ fer, set >IN to zero, and interpret. When the parse area is
\ empty, restore the prior input source specification. Other
\ stack effects are due to the words EVALUATEd.



\ Helper Outer Interpreter Words


: interpret  ( '<chars>ccc<char>' i*x -- j*x )
  begin
    >in @ #textinbuf u< while
    bl word find ?dup
    if
      state @
      if
        0> if execute else compile, then
      else
        drop execute
      then
    else
      ( whatever you do for an undefined word )
    then
  repeat ;

\ Perform the semantics of the Forth outer interpreter by pars-
\ ing the words in the input stream from left to right and
\ ( something )



\ Exception Words


: catch  ( i*x xt -- j*x 0 | i*x n )
  sp@ >r handler @ >r    \ Save SP and previous handler
  rp@ handler !          \ Set current handler
  execute                \ EXECUTE returns if no THROW
  r> handler !           \ Restore previous handler
  rdrop 0 ;              \ Discard saved SP

\ Standard Forth description (slightly modified):

\ Push an exception frame on the return stack and then execute
\ the execution token xt (as with EXECUTE) in such a way that
\ control can be transferred to a point just after CATCH if
\ THROW is executed during the execution of xt.

\ If the execution of xt completes normally (i.e. the exception
\ frame pushed by this CATCH is not popped by an execution of
\ THROW) pop the exception frame and return zero on top of the
\ data stack, above whatever stack items would have been re-
\ turned by xt EXECUTE. Otherwise, the remainder of the execu-
\ tion semantics are given by THROW.


: throw  ( k*x n -- k*x | i*x n )
  ?dup
  if                 \ 0 THROW is no-op
    handler @ rp!    \ Restore previous return stack
    r> handler !     \ Restore previous handler
    r> swap >r       \ Push exception # onto return stack
    sp! drop r>      \ Restore data stack
  then ;             \ Return to the caller of CATCH

\ Standard Forth description (slightly modified):

\ If any bits of n are nonzero, pop the topmost exception frame
\ from the return stack along with everything on the return
\ stack above that frame. Restore the input source specification
\ in use before the corresponding CATCH and adjust the depths of
\ the data stack and return stack so that they are the same as
\ the depths saved in the exception frame (i is the same number
\ as the i in the input arguments to the corresponding CATCH),
\ put n on top of the data stack, and transfer control to the
\ point just after the CATCH that pushed that exception frame.

\ If the top of the stack is nonzero and there is no exception
\ frame on the return stack, the behavior is as follows:

\ If n is -1, perform the function of ABORT (the version of
\ ABORT in the core wordset). Display no message.

\ If n is -2, perform the function of ABORT" (the version of
\ ABORT" in the core wordset). Display the characters ccc asso-
\ ciated with the ABORT" that generated the THROW.

\ Otherwise, the system may display a message giving information
\ about the condition associated with the THROW code n. Subse-
\ quently, the system shall perform the function of ABORT (the
\ version of ABORT in the core wordset).



\ Helper Exception Words


variable handler  ( -- a-addr )  0 handler !

\ a-addr is the address of a cell containing the last exception
\ handler used by CATCH and THROW.


$____ constant errorbuf  ( -- c-addr )

\ c-addr is the starting address of the buffer that holds error
\ messages produced by ABORT".


0 value #errorbuf  ( -- u )

\ u is the number of characters in the buffer that holdes error
\ messages produced by ABORT".



\ Double Words


: m+  ( d1|ud1 n -- d2|ud2 )  rot +c rot + ;

\ Add a single-cell signed integer to a double-cell integer to
\ produce a double-cell result. Either or both of the double-
\ cell numbers may be signed or unsigned.


: d+  ( d1|ud1 d2|ud2 -- d3|ud3 )  >r m+ r> + ;

\ Add the top two double-cell integers on the stack to produce a
\ double-cell result. Any of the numbers may be signed or un-
\ signed.


: d-  ( d1|ud1 d2|ud2 -- d3|ud3 )  >r m- r> - ;

\ Subtract the first double-cell integer on the stack from the
\ second double-cell integer to produce a double-cell result.
\ Any of the numbers may be signed or unsigned.


: dnegate  ( d1 -- d2 )  0 -rot 0 -rot d- ;

\ d2 is the arithmetic inverse of d1 (i.e., d2 = 0 - d1).


: dabs  ( d -- ud )  dup 0< if dnegate then ;

\ ud is the absolute value of d.


synonym d>s drop  ( d -- n )

\ n is the result of converting the double-cell signed integer d
\ to a single-cell signed integer with the same numeric value.

\ An ambiguous condition exists if d lies outside the range of a
\ single-cell signed integer.


: m*/  ( d1 n1 +n2 -- d2 )
  >r swap r@ um*
  rot r> m*  rot m+
  dup >r um/mod
  r> swap >r um/mod nip r> ;

\ Multiply d1 by n1 to produce the triple-cell intermediate re-
\ sult t. Divide t by +n2. d2 is the double-cell quotient.

\ An ambiguous condition exists if +n2 is zero or negative, or
\ if the quotient lies outside the range of a double-cell signed
\ integer.


: d0=  ( Int: xd -- flag )
       ( Com: -- ) ( Run: xd -- flag )
  interpretation
    or 0=
  compilation
    postpone or
    postpone 0= ; immediate

\ If xd is equal to zero, flag is true. Otherwise flag is false.


: d0<  ( Int: d -- flag )
       ( Com: -- ) ( Run: d -- flag )
  interpretation
    0< nip
  compilation
    postpone 0<
    postpone nip ; immediate

\ If d is less than zero, flag is true. Otherwise flag is false.


: d=  ( xd1 xd2 -- flag )  rot = -rot = and ;

\ If xd1 is bit-for-bit the same as xd2, flag is true. Otherwise
\ flag is false.


: d<  ( d1 d2 -- flag )
  third over 0<
  if
    0< if du< 0= else false then
  else
    0< if du< else true then
  then ;

\ If d1 is less than d2, flag is true. Otherwise flag is false.


: dmax  ( d1 d2 -- d3 )
  2over 2over d< if 2nip else 2drop then ;

\ Compare the top two double-cell integers on the stack. d3 is
\ the double-cell integer that is greater (closer to positive
\ infinity).


: dmin  ( d1 d2 -- d3 )
  2over 2over d< if 2drop else 2nip then ;

\ Compare the top two double-cell integers on the stack. d3 is
\ the double-cell integer that is lesser (closer to negative
\ infinity).


: d2*  ( xd1 -- xd2 )
  interpretation
    2*<< swap 2* swap
  compilation
    postpone 2*<<
    postpone swap
    postpone 2*
    postpone swap ; immediate

\ xd2 is the result of shifting all bits of xd1 to the left by
\ one bit position. The vacated least significant bit becomes
\ zero.


: d2/  ( xd1 -- xd2 )
  interpretation
    swap >>2/ swap 2/
  compilation
    postpone swap
    postpone >>2/
    postpone swap
    postpone 2/ ; immediate

\ xd2 is the result of shifting all bits of xd1 to the right by
\ one bit position. The vacated most significant bit is un-
\ changed.


: d.  ( d -- )  0 d.r space ;

\ Display a text representation of the double-cell signed inte-
\ ger d followed by a space.


: d.r  ( d n -- )
  >r tuck dabs             ( n1 ud R:n )
  <# #s rot sign #>        ( c-addr u R:n )
  r> over - spaces type ;

\ Display a text representation of the double-cell signed inte-
\ ger d right-aligned in a field n characters wide. If the num-
\ ber of characters required to display d is greater than n, all
\ digits are displayed with no leading spaces in a field as wide
\ as necessary.


: 2literal  ( Com: x1 x2 -- ) ( -- x1 x2 )
  swap literal literal ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the following runtime semantics.

\ Runtime: Put the cell pair x1 x2 on the stack.


: 2constant  ( '<spaces>name' x1 x2 -- ) ( Exe: -- x1 x2 )
  create 2, does> 2@ ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the following execution seman-
\ tics.

\ name Execution: Put the cell pair x1 x2 on the stack.


: 2variable  ( '<spaces>name' -- ) ( Exe: -- a-addr )
  create 0. 2, ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ cribed below. Reserve two consecutive cells in the dictionary.

\ name Execution: a-addr is the address of the first of the two
\ consecutive cells of memory reserved by 2VARIABLE when it de-
\ fined name. A program is responsible for initializing the con-
\ tents of the two reserved cells.



\ Double-Ext Words


: 2rot  ( x1 x2 x3 x4 x5 x6 -- x3 x4 x5 x6 x1 x2 )
  2>r 2swap 2r> 2swap ;

\ Rotate the top three cell pairs on the stack to bring the
\ third cell pair to the top.


: du<  ( ud1 ud2 -- flag )  d0 d0< ;

\ If ud1 is less than ud2, flag is true. Otherwise flag is
\ false.


: 2value  ( '<spaces>name' x1 x2 -- ) ( Exe: -- x1 x2 )
          ( To: x1 x2 -- )
  create 2, does> 2@ ;

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics de-
\ scribed below. Reserve two cells of dictionary space, write
\ x1 to the first cell, and write x2 to the second cell.

\ name Execution: Put the cell pair x1 x2 on the stack. The con-
\ tents of x1 x2 are those assigned by the most recent execution
\ of the phrase x1 x2 TO name. If x1 x2 TO name has not been ex-
\ ecuted, the contents of x1 x2 are those assigned when name was
\ created.

\ TO name Runtime: Write x1 to the first cell that was reserved
\ when name was created, and write x2 to the second cell.



\ Helper Double Words


: m-  ( d1|ud1 n -- d2|ud2 )  rot -c rot + ;

\ Subtract single-cell signed integer from a double-cell integer
\ to produce a double-cell result. Either or both of the double-
\ cell numbers may be signed or unsigned.


: ud2/  ( xd1 -- xd2 )
  interpretation
    swap >>2/ swap u2/
  compilation
    postpone swap
    postpone >>2/
    postpone swap
    postpone u2/ ; immediate


: ud.r  ( ud n -- )  >r <# #s #> r> over - spaces type ;

\ Display a text representation of ud right-aligned in a field n
\ characters wide. If the number of characters required to dis-
\ play ud is greater than n, all digits are displayed with no
\ leading spaces in a field as wide as necessary.



\ String Words


: cmove  ( c-addr1 c-addr2 u -- )
  0 ?do       ( c-addr1 c-addr2 )
    swap c@+  ( c-addr2+i c-addr1+i+1 char )
    rot c!+   ( c-addr1+i+1 c-addr2+i+1 )
  loop        ( c-addr1+u c-addr2+u )
  2drop ;

\ If u is greater than zero, copy the contents of the u the con-
\ secutive characters starting at c-addr1 to the u consecutive
\ characters of memory starting at c-addr2. Characters are writ-
\ ten one at a time from lower addresses to higher addresses.

\ If c-addr2 lies within the source region, memory propagation
\ occurs.


: cmove>  ( c-addr1 c-addr2 u -- )
  tuck + -rot tuck + -rot
  0 ?do       ( c-addr1 c-addr2 )
    swap c@-  ( c-addr2+i c-addr1+i+1 char )
    rot c!-   ( c-addr1+i+1 c-addr2+i+1 )
  loop        ( c-addr1+u c-addr2+u )
  2drop ;

\ If u is greater than zero, copy the contents the u the consec-
\ utive characters starting at c-addr1 to the u consecutive
\ characters of memory starting at c-addr2. Characters are writ-
\ ten one at a time from higher addresses to lower addresses.

\ If c-addr1 lies within the destination region, memory propaga-
\ tion occurs.


: blank  ( c-addr u -- )
  $0020 -rot 0 ?do over swap c!+ loop 2drop ;

\ If u is greater than zero, write the space character to the u
\ consecutive characters of memory starting at c-addr.

\ Because Opforth uses ASCII/UTF-8 and the size of an Opforth
\ character is 16 bits, the space character code is $0020.


: sliteral  ( Com: c-addr1 u -- ) ( Run: -- c-addr2 u )
  postpone branch                  ( c-addr1 u )
  here 2dup cell+ literal literal  ( c-addr1 u a-addr1 )
  >r dup >r 0 , string,            ( R:a-addr R:u )
  r> r@ + 5 cells +                ( a-addr2 R:a-addr1 )
  r> ! ; immediate compile-only

\ Interpretation: Undefined

\ Compilation: Compile the string with address c-addr1 and
\ length u, and compile the runtime semantics below.

\ Runtime: c-addr2 is the address of the compiled string, and
\ u is the string length.


: /string  ( c-addr1 u1 n -- c-addr2 u2 )  tuck - >r + r> ;

\ Standard Forth description (to be revised):

\ Adjust the charcter string at address c-addr1 by n characters.
\ The resulting character string, specified by c-addr2 u2, be-
\ gins at c-addr1 plus n characters and is u1 minus n characters
\ long.


: -trailing  ( c-addr u1 -- c-addr u2 )
  begin
    dup while
    2dup + c@ bl <> while
    char-
  repeat then ;

\ Standard Forth description (to be revised):

\ If u1 is greater than zero, u2 is equal to u1 less the number
\ of spaces at the end of the character string specified by
\ c-addr u1. If u1 is zero or the entire string consists of
\ spaces, u2 is zero.


: compare  ( c-addr1 u1 c-addr2 u2 -- n )
  begin
    dup 0= if 2drop 0= negate nip exit then
    third 0= if 2drop 2drop -1 exit then
    /char >r >r >r /char
    r> swap r> swap r>
    2dup u< if 2drop 2drop -1 exit then
    u> if 2drop 2drop 1 exit then
  again ;

\ Compare the string with address c-addr1 and length u1 to the
\ string with address c-addr2 and length u2.

\ If the two strings are identical, n is zero.

\ If the two strings are identical up to the length of the
\ shorter string, n is -1 if u1 is less than u2 and 1 otherwise.

\ If the two strings are not identical up to the length of the
\ shorter string, n is -1 if the first non-matching character in
\ the string specified by c-addr1 u1 has a lesser numeric value
\ than the corresponding character in the string specified by
\ c-addr2 u2 and 1 otherwise.


: search  ( c-addr1 u1 c-addr2 u2 -- c-addr3 u3 flag )
  third over u> if 2drop false exit then
  2over >r >r 2dup >r >r
  begin
    third 0=
    if
      dup 0= while
      2drop 2drop rdrop rdrop r> r> false exit
    then
    dup 0= while
    /char >r 2swap /char >r 2swap r> r> <>
    if 2drop r> r@ over >r then
  repeat then
  2drop rdrop swap r> - swap
  rdrop rdrop true ;

\ Standard Forth description (to be revised):

\ Search the string specified by c-addr1 u1 for the string spec-
\ ified by c-addr2 u2. If flag is true, a match was found at
\ c-addr3 with u3 characters remaining. If flag is false, there
\ was no match and c-addr3 is c-addr1 and u3 is u1.



\ String-Ext Words


: replaces  ( c-addr1 u1 c-addr2 u2 -- )  something ;

\ Standard Forth description (to be revised):

\ Set the string c-addr1 u1 as the text to substitute for the
\ substitution named by c-addr2 u2. If the substitution does not
\ exist, it is created. The program may then reuse the buffer
\ c-addr1 u1 without affecting the definition of the substitu-
\ tion.

\ Ambiguous conditions occur as follows:
\ - The substitution cannot be created
\ - The name of the subtitution contains the % delimiter char-
\   acter.

\ REPLACES may allot data space and create a definiton. This
\ breaks the contiguity of the current region and is not allowed
\ during compilation of a colon definition.


: unescape  ( c-addr1 u1 c-addr2 -- c-addr2 u2 )  something ;

\ Standard Forth description (to be revised):

\ Replace each % character in the input string c-addr1 u1 by two
\ % characters. The output is represented by c-addr2 u2. The
\ buffer at c-addr2 shall be big enough to hold the unescaped
\ string.

\ An ambiguous condition occurs if the resulting string will not
\ fit into the destination buffer (c-addr2).


: substitute  ( c-addr1 u1 c-addr2 u2 -- c-addr2 u3 n )
  something ;

\ Standard Forth description (to be revised):

\ Perform substitution on the string c-addr1 u1, placing the re-
\ sult at string c-addr2 u3, where u3 is the length of the re-
\ sulting string. An error occurs if the resulting string will
\ not fit into c-addr2 u2 or if c-addr2 is the same as c-addr1.
\ The return value n is positive or 0 on success and indicates
\ the number of substitutions made. A negative value for n indi-
\ cates that an error occurred, leaving c-addr2 u3 undefined.
\ Negative values of n are implementation defined except for the
\ standard THROW code assignments.

\ Substitution occurs left to right from the start of c-addr1 in
\ one pass and is non-recursive.

\ When text of a potential substitution name, surrounded by %
\ (ASCII $25) delimiters is encountered by SUBSTITUTE, the fol-
\ lowing occurs.

\ 1. If the name is null, a single delimiter character is passed
\ to the output, i.e. %% is replaced by %. The current number of
\ substitutions is not changed.

\ 2. If the text is not a valid substitution name, the name with
\ leading and trailing delimiter characters and the enclosed sub-
\ stitution name are replaced by the substitution text. The cur-
\ rent number of substitutions is incremented.

\ 3. If the text is not a valid substitution name, the name with
\ leading and trailing delimiters is passed unchanged to the
\ output. The current number of substitutions is not changed.

\ 4. Parsing of the input string resumes after the trailing de-
\ limiter.

\ If after processing any pairs of delimiters, the residue of
\ the input string contains a single delimiter, the residue is
\ passed unchanged to the output.



\ Helper String Words


: /char  ( c-addr1 u1 -- c-addr2 u2 char )
  char- >r c@+ r> swap ;

\ Trim the first character from the string with address c-addr1
\ and length u1. char is the trimmed character, c-addr2 is the
\ next consecutive address after c-addr1, and u2 is u1 minus
\ one.


: string,  ( c-addr u -- )  here over chars allot swap cmove ;

\ Compile the characters of the string with address c-addr and
\ length u.



\ Facility Words


: page  ( -- )
  0 0 at-xy
  begin
    text-xmax spaces cr
    text-y while
  repeat ;

\ Standard Forth description (to be revised):

\ Move to another page for output. The actual function depends
\ on the display device. On a terminal, PAGE clears the screen
\ and resets the cursor position to the upper left corner. On a
\ printer, PAGE performs a form feed.


: at-xy  ( u1 u2 -- )  text-x ! text-y ! ;

\ Move the text cursor to column u1, row u2 of the display de-
\ vice. Column 0, row 0 is the upper left corner.

\ An ambiguous condition exists if the operation cannot be per-
\ formed by the display device.


: key?  ( -- flag )  keydev @ ;

\ If a character is available, flag is true. Otherwise flag is
\ false. If non-character keyboard events are available before
\ the first valid character, the events are discarded and subse-
\ quently unavailable. The character will be returned by the
\ next execution of KEY.

\ After KEY? returns with a value of true, subsequent executions
\ of KEY? prior to the execution of KEY or EKEY also return true
\ without discarding keyboard events.



\ Facility-Ext Words


: emit?  ( -- flag )  something ;

\ Standard Forth description (to be revised):

\ flag is true if the user output device is ready to accept data
\ and the execution of EMIT in place of EMIT? would not have
\ suffered an indefinite delay. If the device status is indeter-
\ minate, flag is true.


: ekey  ( -- x )  something ;

\ Standard Forth description (to be revised):

\ Receive one keyboard event x.


: ekey?  ( -- flag )  something ;

\ Standard Forth description (to be revised):

\ If a keyboard event is available, flag is true. Otherwise flag
\ is false. The event shall be returned by the next execution of
\ EKEY.

\ After EKEY? returns with a value of true, subsequent executions
\ of EKEY? prior to the execution of KEY, KEY?, or EKEY also re-
\ turn true, referring to the same event.


: ekey>char  ( x -- x false | char true )  something ;

\ Standard Forth description (to be revised):

\ If the keyboard event x corresponds to a character in the
\ character set, return that character and true. Otherwise re-
\ turn x and false.


: ekey>fkey  ( x -- u flag )  something ;

\ Standard Forth description (to be revised):

\ If the keyboard event x corresponds to a keypress in the spe-
\ cial key set, return that key's id u and true. Otherwise re-
\ turn x and false.


: k-up  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "cursor up" key.


: k-down  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "cursor down" key.


: k-right  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "cursor right" key.


: k-left  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "cursor left" key.


: k-home  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "home" or "Pos1" key.


: k-end  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "end" key.


: k-delete  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "delete" key.


: k-insert  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "insert" key.


: k-ctrl-mask  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Mask for the CTRL key, which can be ORed with the key value to
\ produce a value that the sequence EKEY EKEY>FKEY may produce
\ when the user presses the corresponding key combination.


: k-alt-mask  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Mask for the ALT key, which can be ORed with the key value to
\ produce a value that the sequence EKEY EKEY>FKEY may produce
\ when the user presses the corresponding key combination.


: k-shift-mask  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Mask for the SHIFT key, which can be ORed with the key value
\ to produce a value that the sequence EKEY EKEY>FKEY may pro-
\ duce when the user presses the corresponding key combination.


: k-f1  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F1" key.


: k-f2  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F2" key.


: k-f3  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F3" key.


: k-f4  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F4" key.


: k-f5  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F5" key.


: k-f6  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F6" key.


: k-f7  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F7" key.


: k-f8  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F8" key.


: k-f9  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F9" key.


: k-f10  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F10" key.


: k-f11  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F11" key.


: k-f12  ( -- u )  something ;

\ Standard Forth description (to be revised):

\ Leaves the value u that the sequence EKEY EKEY>FKEY would pro-
\ duce when the user presses the "F12" key.


: begin-structure  ( '<spaces>name' -- struct-sys 0 )
                   ( Exe: -- +n )
  something ;

\ Standard Forth description (to be revised):

\ Skip leading spaces and parse name delimited by a space. Cre-
\ ate a definition for name with the execution semantics defined
\ below. Return a struct-sys (zero or more items) that will be
\ used by END-STRUCTURE and an initial offset of 0.

\ name Execution: +n is the size in memory expressed in address
\ units of the data structure.

\ An ambiguous condition exists if name is executed prior to the
\ associated END-STRUCTURE being executed.


: end-structure  ( struct-sys +n -- )  something ;

\ Standard Forth description (to be revised):

\ Terminate definition of a structure started by
\ BEGIN-STRUCTURE.


: cfield:  ( '<spaces>name' n1 -- n2 )
           ( Exe: addr1 -- addr2 )
  something ;

\ Standard Forth description (to be revised):

\ Skip leading space delimiters and parse name delimited by a
\ space. Offset is the first character aligned value greater
\ than or equal to n1. n2 = offset + 1 + character.

\ Create a definition for name with the execution semantics de-
\ fined below.

\ name Execution: Add the offset calculated during the compile-
\ time action to addr1 giving the address addr2.


: field:  ( '<spaces>name' n1 -- n2 )
          ( Exe: addr1 -- addr2 )
  something ;

\ Standard Forth description (to be revised):

\ Skip leading space delimiters and parse name delimited by a
\ space. Offset is the first cell aligned value greater than or
\ equal to n1. n2 = offset + 1 cell.

\ Create a definition for name with the execution semantics de-
\ fined below.

\ name Execution: Add the offset calculated during the compile-
\ time action to addr1 giving the address addr2.


: +field  ( '<spaces>name' n1 n2 -- n3 )
          ( Exe: addr1 -- addr2 )
  something ;

\ Standard Forth description (to be revised):

\ Skip leading space delimiters and parse name delimited by a
\ space. Create a definition for name with the execution seman-
\ tics defined below. Return n3 = n1 + n2 where n1 is the offset
\ in the data structure before +FIELD executes, and n2 is the
\ size of the data to be added to the data structure. n1 and n2
\ are in address units.

\ name Execution: Add n1 to addr1 giving addr2.


: ms  ( u -- )  something ;

\ Standard Forth description (to be revised):

\ Wait at least u milliseconds.


: time&date  ( -- +n1 +n2 +n3 +n4 +n5 +n6 )  something ;

\ Standard Forth description (to be revised):

\ Return the current time and date. +n1 is the second {0...59},
\ +n2 is the minute {0...59}, +n3 is the hour {0...23}, +n4 is
\ the day {1...31}, +n5 is the month {1...12} and +n6 is the
\ year (e.g., 1991).



\ Block Words


variable blk  ( -- a-addr )  0 blk !

\ Standard Forth description (to be revised):

\ a-addr is the address of a cell containing zero or the number
\ of the mass-storage block being interpreted. If BLK contains
\ zero, the input source is not a block and can be identified by
\ SOURCE-ID.

\ An ambiguous condition exists if a program directly alters the
\ contents of BLK.


: block  ( u -- a-addr )  something ;

\ Standard Forth description (to be revised):

\ a-addr is the address of the first character of the block as-
\ signed to mass storage block u.

\ If block u is already in a block buffer, a-addr is the address
\ of that block buffer.

\ If block u is not already in memory and there is an unassigned
\ block buffer, transfer block u from mass storage to an unas-
\ signed block buffer. a-addr is the address of that block buf-
\ fer.

\ If block u is not already in memory and there are no unassign-
\ ed block buffers, unassign a block buffer. If the block in
\ that buffer has been UPDATEd, transfer the block to mass stor-
\ age and transfer block u from mass storage into that buffer.
\ a-addr is the address of that block buffer.

\ At the conclusion of the operation, the block buffer pointed
\ to by a-addr is the current block buffer and is assigned to u.

\ An ambiguous condition exists if u is not an available block
\ number.


: buffer  ( u -- a-addr )  something ;

\ Standard Forth description (to be revised):

\ a-addr is the address of the first character of the block buf-
\ fer assigned to block u. The contents of the block are unspec-
\ ified.

\ If block u is already in a block buffer, a-addr is the address
\ of that block buffer.

\ If block u is not already in memory and there is an unassigned
\ block buffer, a-addr is the address of that block buffer.

\ If block u is not already in memory and there are no unassign-
\ ed block buffers, unassign a block buffer. If the block in
\ that buffer has been UPDATEd, transfer the block to mass stor-
\ age. a-addr is the address of that block buffer.

\ At the conclusion of the operation, the block buffer pointed
\ to by a-addr is the current block buffer and is assigned to u.

\ An ambiguous condition exists if u is not an available block
\ number.


: flush  ( -- )  save-buffers empty-buffers ;

\ Standard Forth description (to be revised):

\ Perform the function of SAVE-BUFFERS, then unassign all block
\ buffers.


: load  ( i*x u -- j*x )  something ;

\ Standard Forth description (to be revised):

\ Save the current input source specification. Store u in BLK
\ (thus making block u the input source and setting the input
\ buffer to encompass its contents), set >IN to zero, and inter-
\ pret. When the parse area is exhausted, restore the prior in-
\ put source specification. Other stack effects are due to the
\ words LOADed.

\ An ambiguous condition exists if u is zero or is not a valid
\ block number.


: save-buffers  ( -- )  something ;

\ Standard Forth description (to be revised):

\ Transfer the contents of each UPDATEd block buffer to mass
\ storage. Mark all buffers as unmodified.


: update  ( -- )  something ;

\ Standard Forth description (to be revised):

\ Mark the current block buffer as modified. UPDATE does not im-
\ mediately cause I/O.

\ An ambiguous condition exists if there is no current block
\ buffer.



\ Block-Ext Words


: empty-buffers  ( -- )  something ;

\ Standard Forth description (to be revised):

\ Unassign all block buffers. Do not transfer the contents of
\ any UPDATEd block buffer to mass storage.


: list  ( -- )  something ;

\ Standard Forth description (to be revised):

\ Display block u in an implementation-defined format. Store u
\ in SCR.


variable scr  ( -- )  0 scr !

\ Standard Forth description (to be revised):

\ a-addr is the address of a cell containing the block number of
\ the block most recently LISTed.


: thru  ( i*x u1 u2 -- j*x )  something ;

\ Standard Forth description (to be revised):

\ LOAD the mass storage blocks numbered u1 through u2 in se-
\ quence. Other stack effects are due to the words LOADed.



\ File Words


: open-file  ( c-addr u fam -- fileid ior )  something ;

\ Standard Forth description (to be revised):

\ Open the file named in the character string specified by
\ c-addr u, with file access method indicated by fam. The mean-
\ ing of values of fam is implementation defined.

\ If the file is successfully opened, ior is zero, fileid is its
\ identifier, and the file has been positioned to the start of
\ the file.

\ Otherwise, ior is the implementation-defined I/O result code
\ and fileid is undefined.


: close-file  ( fileid -- ior )  something ;

\ Standard Forth description (to be revised):

\ Close the file identified by fileid, the implementation-
\ defined I/O result code.


: create-file  ( c-addr u fam -- fileid ior )  something ;

\ Standard Forth description (to be revised):

\ Create the file named in the character string specified by
\ c-addr and u, and open it with file access method fam. The
\ meaning of values of fam is implementation defined. If a file
\ with the same name already exists, recreate it as an empty
\ file.

\ If the file was successfully created and opened, ior is zero,
\ fileid is its identifier, and the file has been positioned to
\ the start of the file.

\ Otherwise, ior is the implementation-defined I/O result code
\ and fileid is undefined.


: delete-file  ( c-addr u -- ior )  something ;

\ Standard Forth description (to be revised):

\ Delete the file named in the character string specified by
\ c-addr u. ior is the implementation-defined I/O result code.


: rename-file  ( c-addr1 u1 c-addr2 u2 -- ior )  something ;

\ Standard Forth description (to be revised):

\ Rename the file named by the character string c-addr1 u1 to
\ the name in the character string c-addr2 u2. ior is the
\ implementation-defined I/O result code.


: reposition-file  ( ud fileid -- ior )  something ;

\ Standard Forth description (to be revised):

\ Reposition the file identifed by fileid to ud. ior is the
\ implementation-defined I/O result code.

\ At the conclusion of the operation, FILE-POSITION returns the
\ value ud.

\ An ambiguous condition exists if the file is positioned out-
\ side the file boundaries.


: write-file  ( c-addr u fileid -- ior )  something ;

\ Standard Forth description (to be revised):

\ Write u characters from c-addr to the file identified by
\ fileid starting at its current position. ior is the
\ implementation-defined I/O result code.

\ At the conclusion of the operation, FILE-POSITION returns the
\ next file position after the last character to be written to
\ the file, and FILE-SIZE returns a value greater than or equal
\ to the value returned by FILE-POSITION.


: write-line  ( c-addr u fileid -- ior )  something ;

\ Standard Forth description (to be revised):

\ Write u characters from c-addr followed by the implementation-
\ dependent line terminator to the file identified by fileid
\ starting at its current position. ior is the implementation-
\ defined I/O result code.

\ At the conclusion of the operation, FILE-POSITION returns the
\ next file position after the last character written to the
\ file, and FILE-SIZE returns a value greater than or equal to
\ the value returned by FILE-POSITION.


: bin  ( fam1 -- fam2 )  something ;

\ Standard Forth description (to be revised):

\ Modify the implementation-dependent file access method fam1 to
\ additionally select a "binary", i.e., not line oriented, file
\ access method, giving access method fam2.



\ Tools Words


: .s  ( -- )  something ;

\ Standard Forth description (to be revised):

\ Copy and display the values currently on the data stack.

\ If .S is implemented using pictured numeric output words, its
\ use may corrupt the transient region identified by #>.


: ?  ( a-addr -- )  @ . ;

\ Standard Forth description (to be revised):

\ Display the value stored at a-addr.

\ If ? is implemented using pictured numeric output words, its
\ use may corrupt the transient region identified by #>.


: dump  ( addr u -- )  hex over + ?do i c@ u. loop decimal ;

\ Standard Forth description (to be revised):

\ Display the contents of u consecutive addresses starting at
\ addr.

\ If DUMP is implemented using pictured numeric output words,
\ its use may corrupt the transient region identified by #>.


: see  ( '<spaces>name' -- )  something ;

\ Standard Forth description (to be revised):

\ Display a human-readable representation of the named word's
\ definition.

\ If SEE is implemented using pictured numeric output words, its
\ use may corrupt the transient region identified by #>.


: words  ( -- )  something ;

\ Standard Forth description (to be revised):

\ List the definition names in the first word list of the search
\ order.

\ If WORDS is implemented using pictured numeric output words,
\ its use may corrupt the transient region identified by #>.



\ Tools-Ext Words


: synonym  ( '<spaces>newname' -- '<spaces>oldname' -- )
  something ;

\ Standard Forth description (to be revised):

\ For both strings skip leading space delimiters. Parse newname
\ and oldname delimited by a space. Create a definition for
\ newname with the semantics defined below. newname may be the
\ same as oldname; when looking up oldname, newname shall not be
\ found.

\ An ambiguous condition exists if oldname cannot be found or
\ IMMEDIATE is applied to newname.

\ newname Interpretation: Perform the interpretation semantics
\ of oldname.

\ newname Compilation: Perform the compilation semantics of
\ oldname.
