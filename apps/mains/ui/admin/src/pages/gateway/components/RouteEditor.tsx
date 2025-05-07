import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { Box, Typography, Button } from "@mui/material";
import { RefreshCw, Save, Wand2 } from "lucide-react";
import { useRoutes } from "@/hooks/useRoutes";
import toast from "react-hot-toast";
import { Textarea } from "@/components/Textarea";
import { validateJson } from "@/lib/utils";

export const RouteEditor = () => {
  const { t: gateway } = useTranslation("gateway");
  const { t: common } = useTranslation("common");
  const { reload, getJson, updateJson } = useRoutes();
  const [jsonValue, setJsonValue] = useState(getJson.data?.value);
  const textareaRef = useRef<HTMLDivElement>(null);
  useEffect(() => {
    setJsonValue(getJson.data?.value);
  }, []);
  const handleFormat = () => {
    try {
      if (!jsonValue?.trim()) {
        toast.error("JSON 内容为空");
        return;
      }

      const parsed = JSON.parse(jsonValue);
      const formatted = JSON.stringify(parsed, null, 2);
      setJsonValue(formatted);
      toast.success("格式化成功");
    } catch (error) {
      console.error("Format error:", error);
      toast.error("无效的 JSON 格式");
    }
  };
  const handleSave = async () => {
    const validate = validateJson(jsonValue);
    if (!validate.isValid) {
      // 构建详细的错误消息
      let errorMessage = validate.error || "Invalid JSON format";
      // 如果有行号和列号信息，添加到错误消息中
      if (validate.line && validate.column) {
        errorMessage += ` at line ${validate.line}, column ${validate.column}`;
      }

      // 显示错误消息
      toast.error(errorMessage);

      // 如果有文本区域引用和位置信息，设置光标位置到错误处
      const textareaElement = textareaRef.current?.querySelector("textarea");
      if (textareaElement && validate.position) {
        textareaElement.focus();
        textareaElement.setSelectionRange(validate.position, validate.position);
      } else if (textareaElement && validate.line && validate.column) {
        // 如果没有直接的位置信息，但有行和列信息，计算位置
        const lines = jsonValue.split("\n");
        let position = 0;
        for (let i = 0; i < validate.line - 1; i++) {
          position += lines[i].length + 1; // +1 for newline character
        }
        position += validate.column - 1;
        textareaElement.focus();
        textareaElement.setSelectionRange(position, position);
      }
      return;
    }

    try {
      const result = await updateJson.mutateAsync(jsonValue);

      if (result.isFailure) {
        toast.error(result.errors);
        return;
      }

      toast.success(common("saveSuccess"));
    } catch (error) {
      console.error("Save error:", error);
      toast.error(common("saveFailed"));
    }
  };
  const handleReloadConfig = async () => {
    const res = await reload.mutateAsync();
    if (res.isFailure) {
      toast.error(res.errors);
    }
  };
  return (
    <Box
      p={3}
      sx={{
        height: "calc(100vh - 204px)", // 减去顶部导航栏高度
      }}
    >
      {/* 标题和操作按钮 */}
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        mb={3}
      >
        <Typography variant="h5" component="h1">
          {gateway("route.title")}
        </Typography>
        <Box display="flex" gap={2}>
          <Button
            variant="outlined"
            startIcon={<Wand2 size={20} />}
            onClick={handleFormat}
            disabled={getJson.isLoading || updateJson.isPending}
          >
            {gateway("format")}
          </Button>
          <Box display="flex" gap={2}>
            <Button
              disabled={updateJson.isPending || getJson.isLoading}
              onClick={handleSave}
              variant="outlined"
              startIcon={<Save size={20} />}
            >
              {common("save")}
            </Button>
          </Box>
          <Button
            variant="outlined"
            startIcon={
              <RefreshCw
                size={20}
                className={reload.isPending ? "animate-spin" : ""}
              />
            }
            onClick={handleReloadConfig}
            disabled={reload.isPending || getJson.isLoading}
          >
            {reload.isPending
              ? gateway("cluster.reloading")
              : gateway("route.reload")}
          </Button>
        </Box>
      </Box>
      <Textarea
        ref={textareaRef}
        loading={getJson.isLoading}
        value={jsonValue}
        onChange={(e) => setJsonValue(e.target.value)}
        sx={{
          height: "calc(100vh - 64px - 48px - 44px - 56px)", // 减去顶部导航(64px) + padding(48px) + margin(24px) + 标题栏(56px)
          "& .MuiInputBase-root": {
            height: "100%",
          },
          "& .MuiInputBase-input": {
            height: "100% !important",
            resize: "none",
          },
        }}
      />
    </Box>
  );
};
