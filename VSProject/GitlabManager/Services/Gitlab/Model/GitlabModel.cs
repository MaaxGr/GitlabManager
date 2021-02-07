using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitlabManager.Services.Gitlab.Model
{
    
    /// <summary>
    /// Json Classes for serialization of gitlab api.
    /// Currently only project endpoint is used.
    /// 
    /// API classes see https://docs.gitlab.com/ee/api/projects.html
    /// Converter see https://json2csharp.com/
    /// </summary>
    public class JsonNamespace    {
        [JsonProperty("id")]
        public int Id { get; set; } 

        [JsonProperty("name")]
        public string Name { get; set; } 

        [JsonProperty("path")]
        public string Path { get; set; } 

        [JsonProperty("kind")]
        public string Kind { get; set; } 

        [JsonProperty("full_path")]
        public string FullPath { get; set; } 

        [JsonProperty("parent_id")]
        public object ParentId { get; set; } 

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; } 

        [JsonProperty("web_url")]
        public string WebUrl { get; set; } 
    }

    public class JsonLinks    {
        [JsonProperty("self")]
        public string Self { get; set; } 

        [JsonProperty("issues")]
        public string Issues { get; set; } 

        [JsonProperty("merge_requests")]
        public string MergeRequests { get; set; } 

        [JsonProperty("repo_branches")]
        public string RepoBranches { get; set; } 

        [JsonProperty("labels")]
        public string Labels { get; set; } 

        [JsonProperty("events")]
        public string Events { get; set; } 

        [JsonProperty("members")]
        public string Members { get; set; } 
    }

    public class JsonStatistics    {
        [JsonProperty("commit_count")]
        public int CommitCount { get; set; } 

        [JsonProperty("storage_size")]
        public long StorageSize { get; set; } 

        [JsonProperty("repository_size")]
        public int RepositorySize { get; set; } 

        [JsonProperty("wiki_size")]
        public int WikiSize { get; set; } 

        [JsonProperty("lfs_objects_size")]
        public int LfsObjectsSize { get; set; } 

        [JsonProperty("job_artifacts_size")]
        public long JobArtifactsSize { get; set; } 

        [JsonProperty("snippets_size")]
        public int SnippetsSize { get; set; } 

        [JsonProperty("packages_size")]
        public int PackagesSize { get; set; } 
    }

    public class JsonGroupAccess    {
        [JsonProperty("access_level")]
        public int AccessLevel { get; set; } 

        [JsonProperty("notification_level")]
        public int NotificationLevel { get; set; } 
    }

    public class JsonPermissions    {
        [JsonProperty("project_access")]
        public object ProjectAccess { get; set; } 

        [JsonProperty("group_access")]
        public JsonGroupAccess GroupAccess { get; set; } 
    }

    public class JsonProject    {
        [JsonProperty("id")]
        public int Id { get; set; } 

        [JsonProperty("description")]
        public string Description { get; set; } 

        [JsonProperty("name")]
        public string Name { get; set; } 

        [JsonProperty("name_with_namespace")]
        public string NameWithNamespace { get; set; } 

        [JsonProperty("path")]
        public string Path { get; set; } 

        [JsonProperty("path_with_namespace")]
        public string PathWithNamespace { get; set; } 

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; } 

        [JsonProperty("default_branch")]
        public string DefaultBranch { get; set; } 

        [JsonProperty("tag_list")]
        public List<string> TagList { get; set; } 

        [JsonProperty("ssh_url_to_repo")]
        public string SshUrlToRepo { get; set; } 

        [JsonProperty("http_url_to_repo")]
        public string HttpUrlToRepo { get; set; } 

        [JsonProperty("web_url")]
        public string WebUrl { get; set; } 

        [JsonProperty("readme_url")]
        public object ReadmeUrl { get; set; } 

        [JsonProperty("avatar_url")]
        public object AvatarUrl { get; set; } 

        [JsonProperty("forks_count")]
        public int ForksCount { get; set; } 

        [JsonProperty("star_count")]
        public int StarCount { get; set; } 

        [JsonProperty("last_activity_at")]
        public DateTime LastActivityAt { get; set; } 

        [JsonProperty("namespace")]
        public JsonNamespace Namespace { get; set; } 

        [JsonProperty("_links")]
        public JsonLinks Links { get; set; } 

        [JsonProperty("packages_enabled")]
        public object PackagesEnabled { get; set; } 

        [JsonProperty("empty_repo")]
        public bool EmptyRepo { get; set; } 

        [JsonProperty("archived")]
        public bool Archived { get; set; } 

        [JsonProperty("visibility")]
        public string Visibility { get; set; } 

        [JsonProperty("resolve_outdated_diff_discussions")]
        public object ResolveOutdatedDiffDiscussions { get; set; } 

        [JsonProperty("container_registry_enabled")]
        public string ContainerRegistryEnabled { get; set; } 

        [JsonProperty("issues_enabled")]
        public bool IssuesEnabled { get; set; } 

        [JsonProperty("merge_requests_enabled")]
        public bool MergeRequestsEnabled { get; set; } 

        [JsonProperty("wiki_enabled")]
        public bool WikiEnabled { get; set; } 

        [JsonProperty("jobs_enabled")]
        public bool JobsEnabled { get; set; } 

        [JsonProperty("snippets_enabled")]
        public bool SnippetsEnabled { get; set; } 

        [JsonProperty("service_desk_enabled")]
        public bool ServiceDeskEnabled { get; set; } 

        [JsonProperty("service_desk_address")]
        public object ServiceDeskAddress { get; set; } 

        [JsonProperty("can_create_merge_request_in")]
        public bool CanCreateMergeRequestIn { get; set; } 

        [JsonProperty("issues_access_level")]
        public string IssuesAccessLevel { get; set; } 

        [JsonProperty("repository_access_level")]
        public string RepositoryAccessLevel { get; set; } 

        [JsonProperty("merge_requests_access_level")]
        public string MergeRequestsAccessLevel { get; set; } 

        [JsonProperty("forking_access_level")]
        public string ForkingAccessLevel { get; set; } 

        [JsonProperty("wiki_access_level")]
        public string WikiAccessLevel { get; set; } 

        [JsonProperty("builds_access_level")]
        public string BuildsAccessLevel { get; set; } 

        [JsonProperty("snippets_access_level")]
        public string SnippetsAccessLevel { get; set; } 

        [JsonProperty("pages_access_level")]
        public string PagesAccessLevel { get; set; } 

        [JsonProperty("operations_access_level")]
        public string OperationsAccessLevel { get; set; } 

        [JsonProperty("analytics_access_level")]
        public string AnalyticsAccessLevel { get; set; } 

        [JsonProperty("emails_disabled")]
        public object EmailsDisabled { get; set; } 

        [JsonProperty("shared_runners_enabled")]
        public bool SharedRunnersEnabled { get; set; } 

        [JsonProperty("lfs_enabled")]
        public bool LfsEnabled { get; set; } 

        [JsonProperty("creator_id")]
        public int CreatorId { get; set; } 

        [JsonProperty("import_status")]
        public string ImportStatus { get; set; } 

        [JsonProperty("import_error")]
        public object ImportError { get; set; } 

        [JsonProperty("open_issues_count")]
        public int OpenIssuesCount { get; set; } 

        [JsonProperty("runners_token")]
        public string RunnersToken { get; set; } 

        [JsonProperty("ci_default_git_depth")]
        public object CiDefaultGitDepth { get; set; } 

        [JsonProperty("ci_forward_deployment_enabled")]
        public object CiForwardDeploymentEnabled { get; set; } 

        [JsonProperty("public_jobs")]
        public bool PublicJobs { get; set; } 

        [JsonProperty("build_git_strategy")]
        public string BuildGitStrategy { get; set; } 

        [JsonProperty("build_timeout")]
        public int BuildTimeout { get; set; } 

        [JsonProperty("auto_cancel_pending_pipelines")]
        public string AutoCancelPendingPipelines { get; set; } 

        [JsonProperty("build_coverage_regex")]
        public object BuildCoverageRegex { get; set; } 

        [JsonProperty("ci_config_path")]
        public object CiConfigPath { get; set; } 

        [JsonProperty("shared_with_groups")]
        public List<object> SharedWithGroups { get; set; } 

        [JsonProperty("only_allow_merge_if_pipeline_succeeds")]
        public bool OnlyAllowMergeIfPipelineSucceeds { get; set; } 

        [JsonProperty("allow_merge_on_skipped_pipeline")]
        public object AllowMergeOnSkippedPipeline { get; set; } 

        [JsonProperty("request_access_enabled")]
        public bool RequestAccessEnabled { get; set; } 

        [JsonProperty("only_allow_merge_if_all_discussions_are_resolved")]
        public bool OnlyAllowMergeIfAllDiscussionsAreResolved { get; set; } 

        [JsonProperty("remove_source_branch_after_merge")]
        public object RemoveSourceBranchAfterMerge { get; set; } 

        [JsonProperty("printing_merge_request_link_enabled")]
        public bool PrintingMergeRequestLinkEnabled { get; set; } 

        [JsonProperty("merge_method")]
        public string MergeMethod { get; set; } 

        [JsonProperty("suggestion_commit_message")]
        public object SuggestionCommitMessage { get; set; } 

        [JsonProperty("statistics")]
        public JsonStatistics Statistics { get; set; } 

        [JsonProperty("auto_devops_enabled")]
        public bool AutoDevopsEnabled { get; set; } 

        [JsonProperty("auto_devops_deploy_strategy")]
        public string AutoDevopsDeployStrategy { get; set; } 

        [JsonProperty("autoclose_referenced_issues")]
        public bool AutocloseReferencedIssues { get; set; } 

        [JsonProperty("repository_storage")]
        public string RepositoryStorage { get; set; } 

        [JsonProperty("permissions")]
        public JsonPermissions Permissions { get; set; } 
    }


}