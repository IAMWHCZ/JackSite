import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  MenuItem,
  Grid,
  IconButton,
  Tooltip,
  Typography,
  Divider,
  Switch,
  FormControlLabel,
} from "@mui/material";
import { X } from "lucide-react";
import { useForm } from "@tanstack/react-form";
import type { ClusterConfig } from "@/types/gateway";
import { useGatewayStore } from "@/stores/gateway";

const defaultClusterConfig: ClusterConfig = {
  clusterId: "",
  loadBalancingPolicy: undefined,
  sessionAffinity: {
    enabled: false,
    policy: undefined,
    affinityKeyName: "",
    failurePolicy: undefined,
    cookie: {
      path: "/",
      httpOnly: true,
      secure: true,
      sameSite: "Lax",
    },
  },
  healthCheck: {
    active: {
      enabled: true,
      interval: "",
      timeout: "",
      policy: "",
      path: "",
    },
    passive: {
      enabled: false,
      policy: "",
      reactivationPeriod: "",
    },
  },
  httpClient: {
    dangerousAcceptAnyServerCertificate: false,
    maxConnectionsPerServer: 1024,
    enableMultipleHttp2Connections: false,
    requestHeaderEncoding: "",
    responseHeaderEncoding: "",
  },
  httpRequest: {
    version: "",
    versionPolicy: undefined,
    activityTimeout: "",
    allowResponseBuffering: false,
  },
  destinations: {},
  metadata: {},
};


export const ClusterDialog = () => {
  const { t } = useTranslation();
  const { openClusterDialog, setOpenClusterDialog, selectedCluster } =
    useGatewayStore();

  const onClose = () => setOpenClusterDialog(false);
  // 处理关闭事件
  const handleClose = (_: {}, reason: "backdropClick" | "escapeKeyDown") => {
    // 只有当不是点击蒙层时才关闭
    if (reason !== "backdropClick") {
      onClose();
    }
  };

  const form = useForm({
    defaultValues: selectedCluster ?? defaultClusterConfig,
    onSubmit: ({ value }) => {
      console.log(value);
    },
  });

  useEffect(() => {
    if (selectedCluster) {
      form.reset(selectedCluster);
    }else{
      form.reset();
    }
  }, [form,selectedCluster]);

  return (
    <Dialog
      open={openClusterDialog}
      onClose={handleClose}
      maxWidth="lg"
      fullWidth
      PaperProps={{
        sx: { maxWidth: "1000px" },
      }}
    >
      <DialogTitle sx={{ m: 0, p: 2, pr: 6 }}>
        {selectedCluster
          ? t("gateway.dialog.title.editCluster")
          : t("gateway.dialog.title.addCluster")}
        <Tooltip title={t("gateway.dialog.close")}>
          <IconButton
            onClick={onClose}
            sx={{
              position: "absolute",
              right: 8,
              top: 8,
              color: (theme) => theme.palette.grey[500],
            }}
          >
            <X size={20} />
          </IconButton>
        </Tooltip>
      </DialogTitle>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          e.stopPropagation();
          form.handleSubmit();
        }}
      >
        <DialogContent>
          {/* 基本信息 */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t("gateway.dialog.section.basic")}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
            <Grid size={6}>
              <form.Field
                name="clusterId"
                validators={{
                  onChange: ({ value }) =>
                    !value
                      ? t("gateway.dialog.form.clusterId.error")
                      : undefined,
                }}
              >
                {({ state, handleChange }) => (
                  <TextField
                    disabled={!!selectedCluster}
                    value={state.value}
                    onChange={({ target: { value } }) => handleChange(value)}
                    label={t("gateway.dialog.form.clusterId.label")}
                    fullWidth
                    required
                    error={!!state.meta.errors}
                    helperText={state.meta.errors}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="loadBalancingPolicy">
                {({ state, handleChange }) => (
                  <TextField
                    select
                    value={state.value}
                    onChange={({ target: { value } }) =>
                      handleChange(
                        value as ClusterConfig["loadBalancingPolicy"]
                      )
                    }
                    label={t("gateway.dialog.form.loadBalancing")}
                    fullWidth
                  >
                    {[
                      "PowerOfTwoChoices",
                      "FirstAlphabetical",
                      "Random",
                      "RoundRobin",
                      "LeastRequests",
                    ].map((item) => (
                      <MenuItem key={item} value={item}>
                        {item}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              </form.Field>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* Session Affinity */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t("gateway.dialog.section.sessionAffinity")}
          </Typography>
          <form.Subscribe
            selector={(state) => state.values.sessionAffinity?.enabled}
          >
            {(isEnable) => (
              <Grid container spacing={2} sx={{ mb: 3 }}>
                <Grid size={6}>
                  <form.Field name="sessionAffinity.enabled">
                    {({ state, handleChange }) => (
                      <FormControlLabel
                        control={
                          <Switch
                            checked={state.value}
                            onChange={({ target: { checked } }) =>
                              handleChange(checked)
                            }
                          />
                        }
                        label={
                          state.value
                            ? t("common.enabled")
                            : t("common.disabled")
                        }
                      />
                    )}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="sessionAffinity.affinityKeyName">
                    {({ state, handleChange }) => (
                      <TextField
                        value={state.value}
                        onChange={({ target: { value } }) =>
                          handleChange(value)
                        }
                        label={t("gateway.dialog.form.sa.keyName")}
                        fullWidth
                        disabled={!isEnable}
                      />
                    )}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="sessionAffinity.policy">
                    {({ state, handleChange }) => (
                      <TextField
                        select
                        value={state.value}
                        disabled={!isEnable}
                        onChange={({ target: { value } }) =>
                          handleChange(value as "Cookie" | "CustomHeader")
                        }
                        label={t("gateway.dialog.form.sa.policy")}
                        fullWidth
                      >
                        <MenuItem value="Cookie">Cookie</MenuItem>
                        <MenuItem value="CustomHeader">Custom Header</MenuItem>
                      </TextField>
                    )}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="sessionAffinity.failurePolicy">
                    {({ state, handleChange }) => (
                      <TextField
                        select
                        value={state.value}
                        onChange={({ target: { value } }) =>
                          handleChange(
                            value as "Redistribute" | "Return503Error"
                          )
                        }
                        label={t("gateway.dialog.form.sa.failurePolicy")}
                        fullWidth
                        disabled={!isEnable}
                      >
                        <MenuItem value="Redistribute">Redistribute</MenuItem>
                        <MenuItem value="Return503Error">
                          Return 503 Error
                        </MenuItem>
                      </TextField>
                    )}
                  </form.Field>
                </Grid>
              </Grid>
            )}
          </form.Subscribe>
          <Divider sx={{ my: 3 }} />
          {/* 健康检查 */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t("gateway.dialog.section.healthCheck")}
          </Typography>
          <form.Subscribe
            selector={(state) => state.values.healthCheck?.active?.enabled}
          >
            {(isEnable) => (
              <Grid container spacing={2} sx={{ mb: 3 }}>
                {/* Active Health Check */}
                <Grid size={6}>
                  <form.Field name="healthCheck.active.enabled">
                    {({ state, handleChange }) => (
                      <FormControlLabel
                        control={
                          <Switch
                            checked={state.value}
                            onChange={({ target: { checked } }) =>
                              handleChange(checked)
                            }
                          />
                        }
                        label={
                          state.value
                            ? t("common.enabled")
                            : t("common.disabled")
                        }
                      />
                    )}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="healthCheck.active.path">
                    {({ state, handleChange }) => (
                      <TextField
                        value={state.value}
                        onChange={({ target: { value } }) =>
                          handleChange(value)
                        }
                        label={t("gateway.dialog.form.healthCheck.active.path")}
                        fullWidth
                        disabled={!isEnable}
                      />
                    )}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="healthCheck.active.interval">
                    {({ state, handleChange }) => {
                      // 将时间字符串转换为秒数
                      const secondsValue = state.value
                        ? parseInt(state.value.split(":")[2])
                        : 0;

                      // 处理输入变化
                      const handleInputChange = (
                        e: React.ChangeEvent<HTMLInputElement>
                      ) => {
                        const seconds = Math.max(
                          0,
                          parseInt(e.target.value) || 0
                        );
                        const formattedTime = `00:00:${seconds.toString().padStart(2, "0")}`;
                        handleChange(formattedTime);
                      };

                      return (
                        <TextField
                          type="number"
                          className="w-full"
                          fullWidth
                          disabled={!isEnable}
                          value={secondsValue}
                          onChange={handleInputChange}
                          label={t(
                            "gateway.dialog.form.healthCheck.active.interval"
                          )}
                          InputProps={{
                            endAdornment: <span>秒</span>,
                          }}
                        />
                      );
                    }}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="healthCheck.active.policy">
                    {({ state, handleChange }) => (
                      <TextField
                        select
                        value={state.value}
                        onChange={(e) => handleChange(e.target.value)}
                        label={t(
                          "gateway.dialog.form.healthCheck.active.policy"
                        )}
                        fullWidth
                        disabled={!isEnable}
                      >
                        <MenuItem value="ConsecutiveFailures">
                          Consecutive Failures
                        </MenuItem>
                        <MenuItem value="ConsecutiveSuccess">
                          Consecutive Success
                        </MenuItem>
                      </TextField>
                    )}
                  </form.Field>
                </Grid>
              </Grid>
            )}
          </form.Subscribe>

          <Divider sx={{ my: 3 }} />

          {/* HTTP Client */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t("gateway.dialog.section.httpClient")}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
            <Grid size={6}>
              <form.Field name="httpClient.dangerousAcceptAnyServerCertificate">
                {({ state, handleChange }) => (
                  <FormControlLabel
                    control={
                      <Switch
                        checked={state.value}
                        onChange={(e) => handleChange(e.target.checked)}
                      />
                    }
                    label={
                      state.value ? t("common.enabled") : t("common.disabled")
                    }
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Subscribe
                selector={(state) =>
                  state.values.httpClient?.dangerousAcceptAnyServerCertificate
                }
              >
                {(isEnabled) => (
                  <form.Field name="httpClient.maxConnectionsPerServer">
                    {({ state, handleChange }) => (
                      <TextField
                        type="number"
                        value={state.value}
                        onChange={(e) => handleChange(Number(e.target.value))}
                        label={t(
                          "gateway.dialog.form.httpClient.maxConnections"
                        )}
                        fullWidth
                        disabled={!isEnabled}
                      />
                    )}
                  </form.Field>
                )}
              </form.Subscribe>
            </Grid>
            <Grid></Grid>
            <Grid size={6}>
              <form.Field name="httpClient.enableMultipleHttp2Connections">
                {({ state, handleChange }) => (
                  <FormControlLabel
                    control={
                      <Switch
                        className="ml-[-15px]"
                        checked={state.value}
                        onChange={(e) => handleChange(e.target.checked)}
                      />
                    }
                    label={
                      state.value ? t("common.enabled") : t("common.disabled")
                    }
                  />
                )}
              </form.Field>
            </Grid>
            <form.Subscribe
              selector={(state) =>
                state.values.httpClient?.enableMultipleHttp2Connections
              }
            >
              {(isEnabled) => (
                <>
                  <Grid size={6}>
                    <form.Field name="httpClient.requestHeaderEncoding">
                      {({ state, handleChange }) => (
                        <TextField
                          value={state.value}
                          disabled={!isEnabled}
                          onChange={(e) => handleChange(e.target.value)}
                          label={t(
                            "gateway.dialog.form.httpClient.requestHeaderEncoding"
                          )}
                          fullWidth
                        />
                      )}
                    </form.Field>
                  </Grid>
                  <Grid size={6}>
                    <form.Field name="httpClient.responseHeaderEncoding">
                      {({ state, handleChange }) => (
                        <TextField
                          disabled={!isEnabled}
                          value={state.value}
                          onChange={(e) => handleChange(e.target.value)}
                          label={t(
                            "gateway.dialog.form.httpClient.responseHeaderEncoding"
                          )}
                          fullWidth
                        />
                      )}
                    </form.Field>
                  </Grid>
                </>
              )}
            </form.Subscribe>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* HTTP Request */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t("gateway.dialog.section.httpRequest")}
          </Typography>
          <form.Subscribe
            selector={(state) =>
              state.values.httpRequest?.allowResponseBuffering
            }
          >
            {(isEnable) => (
              <Grid container spacing={2} sx={{ mb: 3 }}>
                <Grid size={6}>
                  <form.Field name="httpRequest.allowResponseBuffering">
                    {({ state, handleChange }) => (
                      <FormControlLabel
                        control={
                          <Switch
                            checked={state.value}
                            onChange={(e) => handleChange(e.target.checked)}
                          />
                        }
                        label={
                          state.value
                            ? t("common.enabled")
                            : t("common.disabled")
                        }
                      />
                    )}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="httpRequest.version">
                    {({ state, handleChange }) => (
                      <TextField
                        disabled={!isEnable}
                        select
                        value={state.value}
                        onChange={(e) => handleChange(e.target.value)}
                        label={t("gateway.dialog.form.httpRequest.version")}
                        fullWidth
                      >
                        <MenuItem value="1.0">HTTP/1.0</MenuItem>
                        <MenuItem value="1.1">HTTP/1.1</MenuItem>
                        <MenuItem value="2.0">HTTP/2.0</MenuItem>
                      </TextField>
                    )}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="httpRequest.versionPolicy">
                    {({ state, handleChange }) => (
                      <TextField
                        disabled={!isEnable}
                        select
                        value={state.value}
                        onChange={(e) =>
                          handleChange(
                            e.target.value as
                              | "RequestVersionExact"
                              | "RequestVersionOrLower"
                              | "RequestVersionOrHigher"
                          )
                        }
                        label={t(
                          "gateway.dialog.form.httpRequest.versionPolicy"
                        )}
                        fullWidth
                      >
                        <MenuItem value="RequestVersionExact">
                          Request Version Exact
                        </MenuItem>
                        <MenuItem value="RequestVersionOrLower">
                          Request Version Or Lower
                        </MenuItem>
                        <MenuItem value="RequestVersionOrHigher">
                          Request Version Or Higher
                        </MenuItem>
                      </TextField>
                    )}
                  </form.Field>
                </Grid>
                <Grid size={6}>
                  <form.Field name="httpRequest.activityTimeout">
                    {({ state, handleChange }) => {
                      // 将时间字符串转换为秒数
                      const secondsValue = state.value
                        ? parseInt(state.value.split(":")[2])
                        : 0;

                      return (
                        <TextField
                          type="number"
                          className="w-full"
                          disabled={!isEnable}
                          value={secondsValue}
                          onChange={(e) => {
                            const seconds = Math.max(
                              0,
                              parseInt(e.target.value) || 0
                            );
                            // 转换回 00:00:ss 格式
                            handleChange(
                              `00:00:${seconds.toString().padStart(2, "0")}`
                            );
                          }}
                          label={t(
                            "gateway.dialog.form.httpRequest.activityTimeout"
                          )}
                          InputProps={{
                            endAdornment: <span>秒</span>,
                          }}
                          fullWidth
                        />
                      );
                    }}
                  </form.Field>
                </Grid>
              </Grid>
            )}
          </form.Subscribe>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>{t("common.cancel")}</Button>
          <Button
            onSubmit={(e) => {
              e.preventDefault();
              e.stopPropagation();
              form.handleSubmit();
            }}
            type="submit"
            variant="contained"
          >
            {t("common.confirm")}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};
