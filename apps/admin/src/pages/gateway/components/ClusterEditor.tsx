import { useState, useRef, useEffect } from "react";
import { Box, Button, Typography, Skeleton } from "@mui/material";
import { RefreshCw, Save, Wand2 } from "lucide-react";  // 添加 Wand2 图标
import { useTranslation } from "react-i18next";
import { useClusters } from "@/hooks/useClusters";
import toast from "react-hot-toast";
import { Textarea } from "@/components/Textarea";
import { validateJson } from "@/lib/utils";
export const ClusterEditor = () => {
  const { t: gateway } = useTranslation("gateway");
  const { t: common } = useTranslation("common");
  const { reload, getJson, updateJson } = useClusters();
  const [jsonValue, setJsonValue] = useState("");
  const textareaRef = useRef<HTMLDivElement>(null);

  useEffect(()=>{
    setJsonValue(getJson.data?.value)
  },[])
  // 添加格式化函数
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

  const handleReloadConfig = async () => {
    const res = await reload.mutateAsync();
    if (res.isFailure) {
      toast.error(res.errors);
      return;
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

  return (
    <Box
      p={3}
      sx={{
        height: "calc(100vh - 204px)", // 减去顶部导航栏高度
      }}
    >
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        mb={3}
      >
        <Typography variant="h5" component="h1">
          {gateway("cluster.title")}
        </Typography>
        <Box display="flex" gap={2}>
          {/* 添加格式化按钮 */}
          <Button
            variant="outlined"
            startIcon={<Wand2 size={20} />}
            onClick={handleFormat}
            disabled={getJson.isLoading || updateJson.isPending}
          >
            {gateway("format")}
          </Button>
          <Button
            variant="outlined"
            startIcon={<Save size={20} />}
            onClick={handleSave}
            disabled={getJson.isLoading || updateJson.isPending}
          >
            {updateJson.isPending ? common("saving") : common("save")}
          </Button>
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
              : gateway("cluster.reload")}
          </Button>
        </Box>
      </Box>
      <Box
        sx={{
          flex: 1,
          px: 3,
          pb: 3,
          overflow: "hidden",
          display: "flex",
          position: "relative", // 为loading效果添加定位上下文
        }}
      >
        {getJson.isLoading ? (
          <Box
            sx={{
              position: "absolute",
              top: 0,
              left: (theme) => theme.spacing(3),
              right: (theme) => theme.spacing(3),
              bottom: (theme) => theme.spacing(3),
              display: "flex",
              flexDirection: "column",
              gap: 1,
            }}
          >
            <Skeleton
              variant="rectangular"
              width="100%"
              height="100%"
              animation="wave"
            />
          </Box>
        ) : (
          <Textarea
            ref={textareaRef}
            loading={getJson.isLoading || updateJson.isPending}
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
                fontFamily: "monospace",
              },
            }}
          />
        )}
      </Box>
    </Box>
  );
};
