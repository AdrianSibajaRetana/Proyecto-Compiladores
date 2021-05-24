/* lexical grammar */
%lex
%%

\s+                   /* skip whitespace */
[Ss]ume|[Rr]este|[Mm]ultiplique|[Dd]ivida|[Cc]onjuncion|[Dd]isyuncion|[Ii]guales return 'operador'
[Nn]egar return 'operador_unario'
[Dd]eclare|[Aa]signe|[Ii]mprima|[Rr]etorne|[Ii]nvoque return 'comando'
"variable"|"funcion"|"parametros" return 'tipo'
"a" return 'keyword_asignacion'
"con" return 'keyword_parametros'
"que" return 'keyword_funcion'
[Ss]on return 'keyword_igualdad'
[Ss]i|"entonces" return 'control'
","|"y" return 'separador'
"." return 'terminador'
[0-9]+(.[0-9]+)?|\".*\"|"true"|"false" return 'literal'
[a-zA-Z_]+[0-9]* return 'id'
<<EOF>> return 'EOF'
/lex

%start S

%% /* language grammar */

S: PROGRAMA;

PROGRAMA: SENTENCIAS EOF;

SENTENCIAS:
SENTENCIA
| SENTENCIAS SENTENCIA;

LISTA_PARAMETROS:
PARAMETRO
| PARAMETRO separador LISTA_PARAMETROS {
    return $1 + ", " + $3;
};

PARAMETRO: id;

EXPRESIONES:
EXPRESION
| EXPRESION separador EXPRESIONES {
    return $1 + ", " + $3;
};

SENTENCIA:
comando tipo id terminador {
    return "let " + $3 + ";";
}
| comando EXPRESION keyword_asignacion id terminador {
    return $4 + " = " + $2 + ";";
}
| comando EXPRESION terminador {
    return "console.log(" + $2 + ");";
}
| comando tipo id keyword_parametros tipo LISTA_PARAMETROS keyword_funcion SENTENCIAS terminador {
    return "function " + $3 + "(" + $6 + ") { " + $8  + "}";
}
| comando id keyword_parametros tipo EXPRESIONES {
    return $2 + "(" + $5 + ");";
}
| control EXPRESION control SENTENCIAS terminador {
    return "if (" + $2 + ") { " + $4 + " }";
};

EXPRESION:
operador_unario EXPRESION {
    return "!" + $2;
}
| operador EXPRESION separador EXPRESION {
    var f = "";
    if ($1 == "sume" || $1 == "Sume") {
        f = $2 + " + " + $4;
    }
    else if ($1 == "reste" || $1 == "Reste") {
        f = $2 + " - " + $4;
    }
    else if ($1 == "multiplique" || $1 == "Multiplique") {
        f = $2 + " * " + $4;
    }
    else if ($1 == "divida" || $1 == "Divida") {
        f = $2 + " / " + $4;
    }
    else if ($1 == "conjuncion" || $1 == "Conjuncion") {
        f = $2 + " && " + $4;
    }
    else if ($1 == "disyuncion" || $1 == "Disyuncion") {
        f = $2 + " || " + $4;
    }
    else if ($1 == "iguales" || $1 == "Iguales") {
        f = $2 + " === " + $4;
    }
    return f;
}
| literal
| id;
