import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

ShadThemeData lightThemes = ShadThemeData(
  colorScheme: const ShadSlateColorScheme.light(
    background: Color.fromARGB(255, 255, 255, 255),
    primary: Color.fromARGB(255, 255, 255, 255),
    secondary: Color.fromRGBO(52, 85, 74, 1.000),
  ),
  brightness: Brightness.light,
  inputTheme: const ShadInputTheme(
    decoration: ShadDecoration(
      border: ShadBorder(color: Color.fromARGB(255, 67, 66, 66), width: 1),
    ),
  ),
  selectTheme: const ShadSelectTheme(
    decoration: ShadDecoration(
      border: ShadBorder(color: Color.fromARGB(255, 67, 66, 66), width: 1),
    ),
  ),
  cardTheme: const ShadCardTheme(
    backgroundColor: Colors.white10,
  ),
  sheetTheme: const ShadSheetTheme(
    backgroundColor: Color.fromRGBO(90, 122, 112, 1),
    mainAxisAlignment: MainAxisAlignment.start,
  ),
  primaryDialogTheme: const ShadDialogTheme(
    radius: BorderRadius.all(Radius.circular(16)),
  ),
  linkButtonTheme: const ShadButtonTheme(
    textDecoration: TextDecoration.underline,
    foregroundColor: Colors.black,
  ),
  outlineButtonTheme: const ShadButtonTheme(
    backgroundColor: Color.fromARGB(255, 35, 73, 35),
    foregroundColor: Colors.white,
    decoration: ShadDecoration(
      border: ShadBorder(radius: BorderRadius.all(Radius.circular(100))),
    ),
  ),
  secondaryButtonTheme: const ShadButtonTheme(
    backgroundColor: Color.fromARGB(255, 35, 73, 35),
    foregroundColor: Colors.white,
  ),
);

ShadThemeData darkThemes = ShadThemeData(
  colorScheme: const ShadSlateColorScheme.dark(
    background: Color.fromARGB(255, 54, 56, 54),
    primary: Color.fromARGB(255, 54, 56, 54),
    secondary: Color.fromRGBO(52, 85, 74, 1.000),
  ),
  brightness: Brightness.dark,
  inputTheme: const ShadInputTheme(
    decoration: ShadDecoration(
      border: ShadBorder(color: Colors.white54, width: 1),
    ),
  ),
  selectTheme: const ShadSelectTheme(
    decoration: ShadDecoration(
      border: ShadBorder(color: Colors.white54, width: 1),
    ),
  ),
  cardTheme:
      const ShadCardTheme(backgroundColor: Color.fromRGBO(52, 85, 74, 1.000)),
  sheetTheme:
      const ShadSheetTheme(backgroundColor: Color.fromRGBO(52, 85, 74, 1.000)),
  linkButtonTheme: const ShadButtonTheme(
    textDecoration: TextDecoration.underline,
    foregroundColor: Colors.white,
  ),
  secondaryButtonTheme: const ShadButtonTheme(
    backgroundColor: Color.fromARGB(255, 35, 73, 35),
    foregroundColor: Colors.white,
  ),
  outlineButtonTheme: const ShadButtonTheme(
    backgroundColor: Color.fromARGB(255, 35, 73, 35),
    foregroundColor: Colors.white,
    decoration: ShadDecoration(
      border: ShadBorder(radius: BorderRadius.all(Radius.circular(100))),
    ),
  ),
  switchTheme: const ShadSwitchTheme(
      checkedTrackColor: Color.fromRGBO(96, 133, 121, 1),
      thumbColor: Color.fromRGBO(52, 85, 74, 1.000)),
  popoverTheme: const ShadPopoverTheme(),
);
