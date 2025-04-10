# 🧮 WPF Calculator App

A Windows Presentation Foundation (WPF) calculator built in C#, designed to replicate the standard Windows calculator (Standard + partially Programmer mode). This application demonstrates core concepts of desktop application development in WPF, including UI design, memory operations, and persistence of settings.

---

## ⚙️ Features

- ✅ **Basic arithmetic operations**: `+`, `-`, `*`, `/`, `%`, `√`, `x²`, `±`, `1/x`
- ✅ **Memory functions**: `MC`, `MR`, `MS`, `M+`, `M-`, and memory stack display (`M>`)
- ✅ **Editing tools**: `Backspace`, `CE` (clear entry), `C` (clear all)
- ✅ **Clipboard operations**: `Cut`, `Copy`, `Paste` – custom string-based, not using built-in control functionality
- ✅ **Digit grouping**: numbers formatted with group separators based on current culture (`1.000` in RO, `1,000` in EN)
- ✅ **Programmer mode**:
  - Base conversions between Binary (2), Octal (8), Decimal (10), Hexadecimal (16) (the keyboards are disabled based on the current base)
  - Input validation based on selected base
  - Display only is affected by the base; calculations are still in decimal
- ✅ **User settings persistence**:
  - Last used calculator mode (Standard or Programmer)
  - Last used number base in Programmer mode
  - Digit grouping preference
- ✅ **Keyboard and mouse input support**
- ✅ **Non-resizable calculator window**
- ✅ **Read-only result display**
- ✅ **Help > About** menu with student info

> ❗ **Note**: Operation order (precedence) is **not** implemented. The calculator evaluates expressions sequentially as entered (left-to-right, cascade style). For example, `2 + 3 * 4` results in `20` instead of `14`.

---

## 🧪 Technologies Used

- C#
- .NET / WPF
- MVVM pattern (optional)
- CultureInfo for localization formatting
- File I/O for persistence
  
---
**Standard Mode**

![image](https://github.com/user-attachments/assets/f09dbf84-029c-4c8e-ae97-eadf81e8310c)

**Programmer Mode**

![image](https://github.com/user-attachments/assets/c0a74343-3554-4c95-8443-dbc5039d7cd8)


