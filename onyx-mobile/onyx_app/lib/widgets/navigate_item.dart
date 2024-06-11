// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class NavigateItem extends StatelessWidget {
  final String itemText;
  final String route;

  const NavigateItem({
    Key? key,
    required this.itemText,
    required this.route,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ShadButton.ghost(
      text: Text(
        itemText,
        style: const TextStyle(color: Colors.white, fontSize: 20),
        textAlign: TextAlign.left,
      ),
      onPressed: () {
        context.go('/$route');
      },
    );
  }
}
