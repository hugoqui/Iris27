"use strict";
var __assign = (this && this.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var fs = require("fs");
var util_1 = require("util");
var child_process_1 = require("child_process");
var glob = require("glob");
var uglifyJS = require("uglify-js");
var ncp_1 = require("ncp");
var chokidar = require("chokidar");
var stylus = require("stylus");
var stylus_autoprefixer = require("autoprefixer-stylus");
var typescript = require("typescript");
var tsconfig = require("./tsconfig.json");
var pkg = require("./package.json");
var version = "/* flatpickr v" + pkg.version + ",, @license MIT */";
var paths = {
    themes: "./src/style/themes/*.styl",
    style: "./src/style/flatpickr.styl",
    plugins: "./src/plugins",
    l10n: "./src/l10n",
};
function logErr(e) {
    console.error(e);
}
var writeFileAsync = util_1.promisify(fs.writeFile);
var removeFile = util_1.promisify(fs.unlink);
function startRollup(dev) {
    if (dev === void 0) { dev = false; }
    return new Promise(function (resolve, reject) {
        child_process_1.exec("npm run rollup:" + (dev ? "start" : "build"), function (error, stdout, stderr) {
            if (error) {
                console.error("exec error: " + error);
                return reject();
            }
            console.log(stdout);
            console.log(stderr);
            resolve();
        });
    });
}
function resolveGlob(g) {
    return new Promise(function (resolve, reject) {
        glob(g, function (err, files) { return (err ? reject(err) : resolve(files)); });
    });
}
function recursiveCopy(src, dest) {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            return [2, new Promise(function (resolve, reject) {
                    ncp_1.ncp(src, dest, function (err) { return (!err ? resolve() : reject(err)); });
                })];
        });
    });
}
function readFileAsync(path) {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            return [2, new Promise(function (resolve, reject) {
                    fs.readFile(path, function (err, buffer) {
                        err ? reject(err) : resolve(buffer.toString());
                    });
                })];
        });
    });
}
function compile(src, config) {
    if (config === void 0) { config = tsconfig; }
    return typescript.transpile(src, config);
}
function uglify(src) {
    var minified = uglifyJS.minify(src, {
        output: {
            preamble: version,
            comments: false,
        },
    });
    minified.error && console.log(minified.error);
    return minified.code;
}
function buildScripts() {
    return __awaiter(this, void 0, void 0, function () {
        var transpiled, e_1;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    _a.trys.push([0, 2, , 3]);
                    return [4, readFileAsync("./dist/flatpickr.js")];
                case 1:
                    transpiled = _a.sent();
                    writeFileAsync("./dist/flatpickr.min.js", uglify(transpiled)).catch(logErr);
                    return [3, 3];
                case 2:
                    e_1 = _a.sent();
                    logErr(e_1);
                    return [3, 3];
                case 3: return [2];
            }
        });
    });
}
var extrasConfig = __assign({}, tsconfig, { compilerOptions: __assign({}, tsconfig.compilerOptions, { module: "none" }) });
delete extrasConfig.compilerOptions.module;
function buildExtras(folder) {
    return function () {
        return __awaiter(this, void 0, void 0, function () {
            var _this = this;
            var paths;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        console.log("building " + folder + "...");
                        return [4, recursiveCopy("./src/" + folder, "./dist/" + folder)];
                    case 1:
                        _a.sent();
                        return [4, resolveGlob("./dist/" + folder + "/**/*.ts")];
                    case 2:
                        paths = _a.sent();
                        return [4, Promise.all(paths.map(function (p) { return __awaiter(_this, void 0, void 0, function () {
                                var _a, _b, _c;
                                return __generator(this, function (_d) {
                                    switch (_d.label) {
                                        case 0:
                                            _a = writeFileAsync;
                                            _b = [p.replace(".ts", ".js")];
                                            _c = compile;
                                            return [4, readFileAsync(p)];
                                        case 1: return [4, _a.apply(void 0, _b.concat([_c.apply(void 0, [_d.sent(), extrasConfig])]))];
                                        case 2:
                                            _d.sent();
                                            return [2, removeFile(p)];
                                    }
                                });
                            }); }))];
                    case 3:
                        _a.sent();
                        console.log("done.");
                        return [2];
                }
            });
        });
    };
}
function transpileStyle(src, compress) {
    if (compress === void 0) { compress = false; }
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            return [2, new Promise(function (resolve, reject) {
                    stylus(src, {
                        compress: compress,
                    })
                        .include(__dirname + "/src/style")
                        .include(__dirname + "/src/style/themes")
                        .use(stylus_autoprefixer({
                        browsers: pkg.browserslist,
                    }))
                        .render(function (err, css) {
                        return !err ? resolve(css) : reject(err);
                    });
                })];
        });
    });
}
function buildStyle() {
    return __awaiter(this, void 0, void 0, function () {
        var _a, src, src_ie, _b, style, min, ie;
        return __generator(this, function (_c) {
            switch (_c.label) {
                case 0: return [4, Promise.all([
                        readFileAsync(paths.style),
                        readFileAsync("./src/style/ie.styl"),
                    ])];
                case 1:
                    _a = _c.sent(), src = _a[0], src_ie = _a[1];
                    return [4, Promise.all([
                            transpileStyle(src),
                            transpileStyle(src, true),
                            transpileStyle(src_ie),
                        ])];
                case 2:
                    _b = _c.sent(), style = _b[0], min = _b[1], ie = _b[2];
                    writeFileAsync("./dist/flatpickr.css", style).catch(logErr);
                    writeFileAsync("./dist/flatpickr.min.css", min).catch(logErr);
                    writeFileAsync("./dist/ie.css", ie).catch(logErr);
                    return [2];
            }
        });
    });
}
var themeRegex = /themes\/(.+).styl/;
function buildThemes() {
    return __awaiter(this, void 0, void 0, function () {
        var themePaths;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4, resolveGlob("./src/style/themes/*.styl")];
                case 1:
                    themePaths = _a.sent();
                    themePaths.forEach(function (themePath) {
                        var match = themeRegex.exec(themePath);
                        if (!match)
                            return;
                        readFileAsync(themePath)
                            .then(transpileStyle)
                            .then(function (css) { return writeFileAsync("./dist/themes/" + match[1] + ".css", css); });
                    });
                    return [2];
            }
        });
    });
}
function setupWatchers() {
    watch("./src/plugins", buildExtras("plugins"));
    watch("./src/style/*.styl", function () {
        buildStyle();
        buildThemes();
    });
    watch("./src/style/themes", buildThemes);
}
function watch(path, cb) {
    chokidar
        .watch(path, {
        awaitWriteFinish: {
            stabilityThreshold: 100,
        },
    })
        .on("change", cb)
        .on("error", logErr);
}
function start() {
    var devMode = process.argv.indexOf("--dev") > -1;
    startRollup(devMode).then(buildScripts);
    if (devMode) {
        setupWatchers();
    }
    else {
        buildStyle();
        buildThemes();
        buildExtras("l10n")();
        buildExtras("plugins")();
    }
}
start();
process.on("unhandledRejection", logErr);
