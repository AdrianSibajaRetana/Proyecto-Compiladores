function compilerParse(input) {
    try {
        return {"fueExitoso": true, "salida": compiler.parse(input)};
    } catch (e) {
        return {"fueExitoso": false, "salida": e.message || e};
    }
}